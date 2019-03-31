using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    MapController Map_Controller;
    public int MapSize = 10;
    public GameObject currentPlayer;

    void Start() {
        Map_Controller = GameObject.Find("Map").GetComponent<MapController>();
        Map_Controller.generateFieldBlueprint(MapSize);
        Map_Controller.generateField();
        currentPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>().setMyTurn(true);
    }

    void Update() {

    }
}
