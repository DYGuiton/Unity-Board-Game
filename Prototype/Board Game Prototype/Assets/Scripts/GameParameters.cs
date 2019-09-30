using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameParameters {

    public int playerCount = 0;
    public int mapSize = 0;

    public GameParameters(int playerCount, int mapSize) {
        this.playerCount = playerCount;
        this.mapSize = mapSize;
    }


}
