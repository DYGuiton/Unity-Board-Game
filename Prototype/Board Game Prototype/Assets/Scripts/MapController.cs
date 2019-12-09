using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public int mapSize { get; protected set; }
    public Transform MapTransform;

    public GameObject tileObject;
    public List<GameObject> tilePrefabs;
    public List<Tile> tileList;
    public List<Material> tileMaterials;
    public List<Tile> townTileList;
    private List<TileBlueprint> blueprint;

    public Dictionary<Vector3, Tile> tileCubeCoordinatesMap = new Dictionary<Vector3, Tile>();

    public Dictionary<string, Transform> childDictionary = new Dictionary<string, Transform>();

    public void setMapTransform() {
        foreach (Transform t in transform) {
            childDictionary.Add(t.name, t);
        }
        childDictionary.TryGetValue("Tiles", out MapTransform);
    }

    //Generate a blueprint for a flat hexagonal map
    //This needs to be made into it's own class that extends some basic MapBlueprint class
    public void GenerateFlatHexagonBlueprint(int size) {
        mapSize = size;
        blueprint = new List<TileBlueprint>();
        int radius = size;

        int tileIndex = 0;
        System.Random random = new System.Random();

        for (int q = -radius; q <= radius; q++) {
            int r1 = Math.Max(-radius, -q - radius);
            int r2 = Math.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++) {
                TileBlueprint tileBlueprint = new TileBlueprint(q, r, -q - r);
                tileBlueprint.Index = tileIndex;

                //The following code needs to be made into its own method

                //Designates the Demon Lord Castle Tile
                if (tileBlueprint.Location.magnitude == 0) {
                    tileBlueprint.TileType = 0;
                }
                //Designates all town tiles
                else if (Math.Abs(tileBlueprint.Location.x) == size * 1.5 && Math.Abs(tileBlueprint.Location.z) == size) {
                    tileBlueprint.TileType = 2;
                    tileBlueprint.rotation = new Vector3(0, random.Next(0, 2) * 180, 0);
                }
                //Randomize all other tiles
                else {
                    tileBlueprint.TileType = random.Next(3, 8);
                    tileBlueprint.rotation = new Vector3(0, random.Next(0, 2) * 180, 0);
                }
                blueprint.Add(tileBlueprint);
                tileIndex++;
            }
        }
    }

    public void GenerateFlatHexField() {
        if (blueprint == null) {
            Debug.Log("No blueprint to go off of.");
            return;
        }

        tileList = new List<Tile>();
        GameObject nuTileObject;
        Tile nuTile;

        /* Tile Types:
        * 0. Demon Lords Castle
        * 1. Road
        * 2. Town
        * 3. Dryland
        * 4. Forest
        * 5. Mine
        * 6. Plains
        * 7. Sea
        *
        * Major Tile: 0-2
        * Minor Tiles: 3-7
        */
        foreach (TileBlueprint tileBlueprint in blueprint) {
            nuTileObject = Instantiate(tilePrefabs[tileBlueprint.TileType], tileBlueprint.Location, Quaternion.identity * Quaternion.Euler(tileBlueprint.rotation));
            nuTile = nuTileObject.GetComponent<Tile>();

            if (tileBlueprint.TileType == 2) {
                townTileList.Add(nuTileObject.GetComponent<Tile>());
            }
            nuTile.copyBlueprint(tileBlueprint);
            nuTile.name = "Tile #" + nuTileObject.GetComponent<Tile>().Index;
            nuTile.transform.SetParent(MapTransform);
            tileList.Add(nuTile);
            tileCubeCoordinatesMap.Add(nuTileObject.GetComponent<Tile>().cubeCoordinates, nuTile);
        }

        SetAllTileNighbours();

    }

    private void SetAllTileNighbours() {
        foreach (var tile in tileList) {
            tile.setNeighboursList(tileCubeCoordinatesMap);
        }
    }
}
