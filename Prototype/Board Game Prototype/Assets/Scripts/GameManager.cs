using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update

    MapController Map_Controller;
    public int Size = 10;


    void Start() {
        Map_Controller = GameObject.Find("Map").GetComponent<MapController>();
        Map_Controller.generateField(Size);
    }

    // Update is called once per frame
    void Update() {

    }
}
