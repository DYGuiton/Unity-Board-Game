using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public Transform MapTransform;

    public GameObject tileObject;
    public List<GameObject> tilePrefabs;
    public List<Tile> tileList;
    public List<Material> tileMaterials;
    public List<Tile> tileTypes;
    private List<TileBlueprint> blueprint;

    public Dictionary<string, Transform> childDictionary = new Dictionary<string, Transform>();

    void Start() {
        foreach (Transform t in transform) {
            childDictionary.Add(t.name, t);
        }
        childDictionary.TryGetValue("Tiles", out MapTransform);
    }

    public void generateFieldBlueprint(int size) {
        blueprint = new List<TileBlueprint>();
        int largestLine = size * 2 - 1;
        int length = largestLine;
        int layer = 0;

        //generate blueprints for center line Tiles
        for (int i = 0; i < largestLine; i++) {
            blueprint.Add(new TileBlueprint(new Vector3(0, 0, i * 2)));
        }

        //generate blueprints for all outer line Tiles
        for (int i = 1; i <= largestLine - size; i++) {
            length--;
            layer++;
            for (int j = 0; j < length; j++) {
                blueprint.Add(new TileBlueprint(new Vector3(i * 1.50f, 0, 2 * j + layer)));
                blueprint.Add(new TileBlueprint(new Vector3(-i * 1.50f, 0, 2 * j + layer)));
            }
        }

        //sort the list
        blueprint.Sort(TileBlueprintComparer.sortDescendingXPosition());

        //Set blueprint indices 
        for (int i = 0; i < blueprint.Count; i++) {
            blueprint[i].Index = i;
        }

        //set blueprint tile types
        setTileTypeBlueprints(size);
    }

    private void setTileTypeBlueprints(int size) {
        //Set Tile blueprints for Types 0-2
        setMajorTileTypeBlueprints(size);
        //Set Tile blueprints for Types 3-7
        setMinorTileTypeBlueprints(size);
    }

    private void setMajorTileTypeBlueprints(int size) {

        int inc = size;
        int distance = size - 1;
        int centerIndex = blueprint.Count / 2;

        //Set Tile Type for all potential Road Tile blueprints (Type 1)
        for (int i = 0; i < blueprint.Count; i = i + inc) {

            TileBlueprint negative_slope_tile = blueprint[i];
            TileBlueprint positive_slope_tile = blueprint[i + distance];
            negative_slope_tile.isMajor_Tile = true;
            positive_slope_tile.isMajor_Tile = true;

            negative_slope_tile.TileType = positive_slope_tile.TileType = 1;
            if (negative_slope_tile.Index < centerIndex) {
                inc++;
            }
            if (negative_slope_tile.Index > centerIndex) {
                inc--;
            }
            distance--;
        }

        //Mark all Town Tile blueprints (Type 2)
        int[] townTiles = { 0, size - 1, blueprint.Count - size, blueprint.Count - 1 };
        foreach (int townIndex in townTiles) {
            blueprint[townIndex].TileType = 2;
        }

        //Mark Demon Lord Castle Tile
        blueprint[blueprint.Count / 2].TileType = 0;

    }

    private void setMinorTileTypeBlueprints(int size) {
        System.Random random = new System.Random();
        foreach (var tileBlueprint in blueprint) {
            if (!tileBlueprint.isMajor_Tile) {
                tileBlueprint.TileType = random.Next(3, 8);
            }
        }
    }

    public void generateField() {
        if (blueprint == null) {
            Debug.Log("No blueprint to go off of.");
            return;
        }

        tileList = new List<Tile>();
        GameObject nuTile;

        foreach (TileBlueprint tileBlueprint in blueprint) {
            switch (tileBlueprint.TileType) {
                case 0:
                    nuTile = Instantiate(tilePrefabs[0], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<DemonLordCastleTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[0];
                    break;
                case 1:
                    nuTile = Instantiate(tilePrefabs[1], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<RoadTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[1];
                    break;
                case 2:
                    nuTile = Instantiate(tilePrefabs[2], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<TownTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[2];
                    break;
                case 3:
                    nuTile = Instantiate(tilePrefabs[3], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<DrylandTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[3];
                    break;
                case 4:
                    nuTile = Instantiate(tilePrefabs[4], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<ForestTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[4];
                    break;
                case 5:
                    nuTile = Instantiate(tilePrefabs[5], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<MineTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[5];
                    break;
                case 6:
                    nuTile = Instantiate(tilePrefabs[6], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<PlainsTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[6];
                    break;
                case 7:
                    nuTile = Instantiate(tilePrefabs[7], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<SeaTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[7];
                    break;
                default:
                    nuTile = Instantiate(tilePrefabs[0], tileBlueprint.Location, Quaternion.identity);
                    nuTile.AddComponent<RoadTile>();
                    nuTile.transform.GetChild(0).GetComponent<MeshRenderer>().material = tileMaterials[1];
                    break;
            }
            nuTile.GetComponent<Tile>().copyBlueprint(tileBlueprint);
            nuTile.name = "Tile #" + nuTile.GetComponent<Tile>().Index;
            nuTile.transform.SetParent(MapTransform);
            tileList.Add(nuTile.GetComponent<Tile>());
        }

    }
}
