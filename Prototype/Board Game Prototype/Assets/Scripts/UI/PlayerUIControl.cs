﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    public GameObject playerNameLabel;

    public GameObject selectedObject;
    public GameObject bottomRightUI;

    public void Setup(GameObject newPlayer) {
        playerNameLabel.GetComponent<TextMeshProUGUI>().text = newPlayer.name;
        gameObject.SetActive(false);


    }

    public void TurnOn() {
        gameObject.SetActive(true);
        //close all superfluous things
    }

    public void HeroSelected(HeroControl hero, bool isMyHero) {
        selectedObject = hero.gameObject;

        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;

        ActionMenu.SetActive(true);
        InfoMenu.SetActive(true);

        if(isMyHero == true) {
            ActionMenu.transform.Find("MoveButton").gameObject.SetActive(true);
        }
        ActionMenu.transform.Find("DeselectButton").gameObject.SetActive(true);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = "Hero";
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = hero.material.name;


    }

    public void HeroDeselected(HeroControl hero) {
        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;


        ActionMenu.transform.Find("MoveButton").gameObject.SetActive(false);
        ActionMenu.transform.Find("DeselectButton").gameObject.SetActive(false);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = "";
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = "";

        ActionMenu.SetActive(false);
        InfoMenu.SetActive(false);
    }

    public void TileSelected(Tile tile) {
        selectedObject = tile.gameObject;
        
        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;

        ActionMenu.SetActive(true);
        InfoMenu.SetActive(true);

        ActionMenu.transform.Find("DeselectButton").gameObject.SetActive(true);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = tile.gameObject.name;
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = tile.gameObject.transform.position.ToString();
    }

    public void TileDeselected(Tile tile) {
        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;

        ActionMenu.transform.Find("DeselectButton").gameObject.SetActive(false);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = "";
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = "";

        ActionMenu.SetActive(false);
        InfoMenu.SetActive(false);
    }

    public void Deselection() {
        if(selectedObject.tag == "Hero") {
            HeroDeselected(selectedObject.GetComponent<HeroControl>());
        }
        else if (selectedObject.tag == "Tile") {
            TileDeselected(selectedObject.GetComponent<Tile>());
        }

        selectedObject = null;
    }
}