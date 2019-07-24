using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerManager {

    public GameObject playerHolder;
    public List<GameObject> playerObjectsList = new List<GameObject>();
    public GameObject playerPrefab;

    internal GameObject CreatePlayer(Tile townTile) {

        GameObject newPlayer = GameObject.Instantiate(playerPrefab);
        PlayerControl newPlayerController = newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>();

        newPlayerController.setTownTile(townTile);
        playerObjectsList.Add(newPlayer);
        newPlayerController.id = playerObjectsList.IndexOf(newPlayer);
        newPlayer.transform.SetParent(playerHolder.transform);

        return newPlayer;
    }
}
