using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TileBlueprint {
    public int Index { get; set; }
    public int TileType { get; set; }
    public Vector3 Location { get; set; }
    public bool isMajor_Tile { get; set; }

    public TileBlueprint() {
    }

    public TileBlueprint(Vector3 location) {
        this.Location = location;
    }

}
