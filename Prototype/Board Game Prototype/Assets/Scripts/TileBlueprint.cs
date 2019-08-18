using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileBlueprint {
    public int Index { get; set; }
    public int rowLength { get; set; }
    public int TileType { get; set; }
    public Vector3 Location { get; set; }
    public Vector3 rotation { get; set; }
    public bool isMajor_Tile { get; set; }

    public int q, r, s;

    public TileBlueprint(int q, int r, int s) {
        this.q = q;
        this.r = r;
        this.s = s;
        if (q + r + s != 0) throw new ArgumentException("q + r + s must be 0");
        SetPixelLocation();
    }

    public TileBlueprint(int q, int r) {
        this.q = q;
        this.r = r;
        s = (-q - r);
        SetPixelLocation();
    }

    public TileBlueprint(Vector3 location) {
        this.Location = location;
    }

    private void SetPixelLocation() {
        float x = q * 1.5f;
        float z = -s+r;
        Location = new Vector3(x, 0, z);


    }

}
