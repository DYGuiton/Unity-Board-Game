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
    public int rowLength { get; set; }
    public Vector3 location { get; set; }
    public Vector3 rotation { get; set; }
    public bool isMajor_Tile { get; set; }
    public Material originalMaterial;

    public int q, r, s;

    public List<Tile> neighbours = new List<Tile>();

    public void copyBlueprint(TileBlueprint myBlueprint) {
        Index = myBlueprint.Index;
        TileType = myBlueprint.TileType;
        rowLength = myBlueprint.rowLength;
        location = myBlueprint.Location;
        rotation = myBlueprint.rotation;
        isMajor_Tile = myBlueprint.isMajor_Tile;
        originalMaterial = new Material(transform.GetComponentInChildren<MeshRenderer>().material);
    }

    public void SetCubeCoordinates(int q, int r, int s) {
        this.q = q;
        this.r = r;
        this.s = s;
        if (q + r + s != 0) throw new ArgumentException("q + r+ s must be 0");
    }

    public void SetAxialCoordinates(int q, int r) {
        this.q = q;
        this.r = r;
        s = (-q - r);
    }


    public void setNeighboursList(List<Tile> tileList) {
        int[] neighbourIndeces = { Index - rowLength, Index - rowLength + 1, Index - 1, Index + 1, Index + rowLength, Index + rowLength + 1 };
        for (int i = 0; i < neighbourIndeces.Length; i++) {
            if (neighbourIndeces[i] >= 0 && neighbourIndeces[i] < tileList.Count) {
                neighbours.Add(tileList[neighbourIndeces[i]]);

            }
        }

    }

    public void Highlight() {
        Material highlightTileMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
        highlightTileMaterial.shader = Shader.Find("Outlined/UltimateOutline");
        highlightTileMaterial.SetColor("_FirstOutlineColor", new Color(255, 207, 0, 1));
        highlightTileMaterial.SetFloat("_FirstOutlineWidth", 0.1f);
        highlightTileMaterial.SetColor("_SecondOutlineColor", new Color(255, 207, 0, 0));
        highlightTileMaterial.SetFloat("_SecondOutlineWidth", 0.0f);
    }

    public void Unhighlight() {
        gameObject.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
    }

}
