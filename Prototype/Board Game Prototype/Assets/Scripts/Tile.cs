using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IComparable<Tile> {

    public int Index { get; set; }
    public bool isMajor_Tile { get; set;}

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public int CompareTo(Tile other) {
        if (other == null) {
            return 1;
        }
        else {

            //Compare two Tiles and return the leftmost one
            int compare_value = gameObject.transform.position.x.CompareTo(other.gameObject.transform.position.x);
            compare_value = compare_value * -1;

            //If the Tiles are in the same row, return the highest one
            if(compare_value == 0) {
                compare_value = gameObject.transform.position.z.CompareTo(other.gameObject.transform.position.z);
            }

            return compare_value;
        }
    }
}
