using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public GameObject Tile;
    public List<Tile> Tile_List;
    public List<int> MajorTile_List;
    public List<Material> Tile_Materials;

    public Dictionary<string, Transform> Child_Dictionary = new Dictionary<string, Transform>();


    // Start is called before the first frame update
    void Start() {
        foreach (Transform t in transform) {
            Child_Dictionary.Add(t.name, t);
        }
    }

    public void generateField(int size) {
        Tile_List = new List<Tile>();
        int largestLine = size * 2 - 1;
        int length = largestLine;
        int layer = 0;
        int currentIndex = 0;
        GameObject nuTile;

        //generate the center line Tiles
        for (int i = 0; i < largestLine; i++) {
            nuTile = Instantiate(Tile, new Vector3(0, 0, i * 2), Quaternion.identity);
            Tile_List.Add(nuTile.GetComponent<Tile>());
        }

        //generate all outer line Tiles
        for (int i = 1; i <= largestLine - size; i++) {
            length--;
            layer++;
            for (int j = 0; j < length; j++) {
                nuTile = Instantiate(Tile, new Vector3(i * 1.50f, 0, 2 * j + layer), Quaternion.identity);
                Tile_List.Add(nuTile.GetComponent<Tile>());
                nuTile = Instantiate(Tile, new Vector3(-i * 1.50f, 0, 2 * j + layer), Quaternion.identity);
                Tile_List.Add(nuTile.GetComponent<Tile>());
            }
        }

        //sort the list
        Tile_List.Sort();


        //Set indices and store Tile GameObject into the Map Object
        Transform tile_parent;
        Child_Dictionary.TryGetValue("Tiles", out tile_parent);
        for (int i = 0; i < Tile_List.Count; i++) {
            Tile_List[i].GetComponent<Tile>().Index = currentIndex++;
            Tile_List[i].gameObject.transform.SetParent(tile_parent); ;
        }

        setMajorTiles(size);
        setMinorTiles(size);

    }

    private void setMajorTiles(int size) {
        int inc = size;
        int distance = size - 1;
        int centerIndex = Tile_List.Count / 2;

        //Mark all potential road tiles
        for (int i = 0; i < Tile_List.Count; i = i + inc) {
            Tile negative_slope_tile = Tile_List[i];
            Tile positive_slope_tile = Tile_List[i + distance];
            negative_slope_tile.isMajor_Tile = true;
            positive_slope_tile.isMajor_Tile = true;
            negative_slope_tile.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = Tile_Materials[2];
            positive_slope_tile.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = Tile_Materials[2];
            MajorTile_List.Add(i);
            MajorTile_List.Add(i + distance);
            if (negative_slope_tile.Index < centerIndex) {
                inc++;
            }
            if (negative_slope_tile.Index > centerIndex) {
                inc--;
            }
            distance--;
        }

        //Mark all Town Tiles
        int[] townTiles = { 0, size - 1, Tile_List.Count - size, Tile_List.Count - 1};
        foreach (int townIndex in townTiles) {
            Tile_List[townIndex].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = Tile_Materials[1];
        }

        //Mark Demon Lord Castle Tile
        Tile_List[Tile_List.Count/2].gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = Tile_Materials[0];



    }



    private void setMinorTiles(int size) {
        System.Random random = new System.Random();
        foreach (var Tile in Tile_List) {
            if (!Tile.isMajor_Tile) {
                Tile.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = Tile_Materials[random.Next(3, 8)];
            }
        }
    }
}
