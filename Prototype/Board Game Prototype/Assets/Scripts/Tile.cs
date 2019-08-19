using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : BoardObject {
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

    public int Index;
    public int TileType;
    public Vector3 location;
    public Vector3 rotation;

    public Vector3 cubeCoordinates;
    int q, r, s;

    public List<Tile> neighbors = new List<Tile>();

    public void copyBlueprint(TileBlueprint myBlueprint) {
        Index = myBlueprint.Index;
        TileType = myBlueprint.TileType;
        location = myBlueprint.Location;
        rotation = myBlueprint.rotation;
        originalMaterial = new Material(transform.GetComponentInChildren<MeshRenderer>().material);
        SetCubeCoordinates(myBlueprint.q, myBlueprint.r, myBlueprint.s);
    }

    public void SetCubeCoordinates(int q, int r, int s) {
        this.q = q;
        this.r = r;
        this.s = s;
        if (q + r + s != 0) throw new ArgumentException("q + r + s must be 0");
        cubeCoordinates = new Vector3(q, r, s);
    }

    public void SetAxialCoordinates(int q, int r) {
        this.q = q;
        this.r = r;
        s = (-q - r);
    }


    public void setNeighboursList(Dictionary<Vector3, Tile> tileCubeCoordinatesMap) {
        Tile neighbor;

        Vector3[] neighborVectors = {
            new Vector3(q + 1, r - 1, s + 0), new Vector3(q + 1, r + 0, s - 1), new Vector3(q + 0, r + 1, s - 1),
            new Vector3(q - 1, r + 1, s + 0), new Vector3(q - 1, r + 0, s + 1), new Vector3(q + 0, r - 1, s + 1)
        };

        for (int i = 0; i < neighborVectors.Length; i++) {
            if(tileCubeCoordinatesMap.TryGetValue(neighborVectors[i], out neighbor)) {
                neighbors.Add(neighbor);
            }
        }
    }

    public override bool Highlight() {
        Material highlightTileMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
        highlightTileMaterial.shader = Shader.Find("Outlined/UltimateOutline");
        highlightTileMaterial.SetColor("_FirstOutlineColor", new Color(1.0f, 1.0f, 0.0f, 1.0f));
        highlightTileMaterial.SetFloat("_FirstOutlineWidth", 0.1f);
        highlightTileMaterial.SetColor("_SecondOutlineColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));
        highlightTileMaterial.SetFloat("_SecondOutlineWidth", 0.0f);
        return true;
    }

    public override bool Highlight(Material highlightMaterial) {
        Material currentTileMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
        currentTileMaterial = highlightMaterial;
        return true;
    }

    public override void Unhighlight() {
        gameObject.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
    }

}
