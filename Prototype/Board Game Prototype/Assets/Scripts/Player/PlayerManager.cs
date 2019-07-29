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

    //this is nothing but a method to test how heroes will function within the game. 
    //Players should NOT be given heroes from the playerManager script (I think it should probably be the game manager that does that)
    private void giveHero(PlayerControl newPlayerController) {
        GameObject newHero = GameObject.Instantiate(heroPrefab);
        newHero.GetComponent<HeroControl>().heroProfile = new HeroProfile(createRandomHeroType());
        newPlayerController.addHero(newHero.GetComponent<HeroControl>());
    }

    private HeroType createRandomHeroType() {
        System.Random dice = new System.Random();
        HeroType heroType;
        switch(dice.Next(0, 4)) {
            case 0:
                heroType = new AnatomistHeroType();
                break;
            case 1:
                heroType = new KnightHeroType();
                break;
            case 2:
                heroType = new MagusHeroType();
                break;
            case 3:
                heroType = new MercenaryHeroType();
                break;
            case 4:
                heroType = new RangerHeroType();
                break;
            default:
                heroType = new AnatomistHeroType();
                break;
        }
        return heroType;
    }
}
