using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerManager : MonoBehaviour {

    public List<Material> playerMaterials;

    public List<GameObject> playerObjectsList = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject heroPrefab;

    internal GameObject CreatePlayer(Tile nuTownTile) {

        GameObject newPlayer = GameObject.Instantiate(playerPrefab);
        PlayerControl newPlayerController = newPlayer.transform.Find("PlayerController").GetComponent<PlayerControl>();

        playerObjectsList.Add(newPlayer);
        newPlayerController.id = playerObjectsList.IndexOf(newPlayer);

        newPlayerController.setTownTile(nuTownTile, playerMaterials[newPlayerController.id]);

        newPlayer.transform.SetParent(gameObject.transform);

        giveHero(newPlayerController);

        return newPlayer;
    }

    private void giveHero(PlayerControl newPlayerController) {
        GameObject newHero = GameObject.Instantiate(heroPrefab);
        newPlayerController.addHero(newHero.GetComponent<HeroControl>());
    }
}
