using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
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

    public int Index { get; set; }
    public int TileType { get; set; }
    public Vector3 location { get; set; }
    public Vector3 rotation { get; set; }
    public bool isMajor_Tile { get; set; }
    public Material material { get; set; }

    public void copyBlueprint(TileBlueprint myBlueprint) {
        Index = myBlueprint.Index;
        TileType = myBlueprint.TileType;
        location = myBlueprint.Location;
        rotation = myBlueprint.rotation;
        isMajor_Tile = myBlueprint.isMajor_Tile;
    }

}
