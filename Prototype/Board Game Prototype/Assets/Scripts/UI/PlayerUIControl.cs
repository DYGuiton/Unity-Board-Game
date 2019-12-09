using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIControl : MonoBehaviour {
    public GameObject playerNameLabel;

    public GameObject selectedObject;
    public GameObject bottomRightUI;
    public Material playerMaterial;

    public delegate void MoveButtonDelegate(GameObject selectedHero);
    public delegate void CancelButtonDelegate();
    public delegate void EndTurnButtonDelegate();
    public event MoveButtonDelegate moveHero;
    public event CancelButtonDelegate cancel;
    public event EndTurnButtonDelegate endTurn;

    public TextMeshProUGUI woodContainer;
    public TextMeshProUGUI foodContainer;
    public TextMeshProUGUI joyContainer;

    public void Start() {
        woodContainer = gameObject.transform.GetChild(0).Find("OverHeadUI").Find("ResourceContainer").Find("Wood").Find("ResourceValue").GetComponent<TextMeshProUGUI>();
        foodContainer = gameObject.transform.GetChild(0).Find("OverHeadUI").Find("ResourceContainer").Find("Food").Find("ResourceValue").GetComponent<TextMeshProUGUI>();
        joyContainer = gameObject.transform.GetChild(0).Find("OverHeadUI").Find("ResourceContainer").Find("Joy").Find("ResourceValue").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
    }

    public void Setup(PlayerControl newPlayerControl) {
        playerNameLabel.GetComponent<TextMeshProUGUI>().text = newPlayerControl.playerName;
        playerMaterial = newPlayerControl.playerMaterial;

        gameObject.transform.GetChild(0).Find("TopRightUI").GetChild(0).GetComponent<Image>().material = playerMaterial;
        gameObject.SetActive(false);

    }

    public void TurnOn() {
        gameObject.SetActive(true);
    }

    public void TurnOff() {
        gameObject.SetActive(false);
    }

    #region SelectionHandling

    public void HeroSelected(HeroControl hero, bool isMyHero) {
        selectedObject = hero.gameObject;

        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;

        ActionMenu.SetActive(true);
        InfoMenu.SetActive(true);

        if (isMyHero == true) {
            ActionMenu.transform.Find("MoveButton").gameObject.SetActive(true);
        }
        ActionMenu.transform.Find("CancelButton").gameObject.SetActive(true);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = "Hero";
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = hero.originalMaterial.name;
    }

    public void HeroDeselected(HeroControl hero) {
        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;


        ActionMenu.transform.Find("MoveButton").gameObject.SetActive(false);
        ActionMenu.transform.Find("CancelButton").gameObject.SetActive(false);

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

        ActionMenu.transform.Find("CancelButton").gameObject.SetActive(true);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = tile.gameObject.name;
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = tile.gameObject.transform.position.ToString();
    }

    public void TileDeselected(Tile tile) {
        GameObject ActionMenu = bottomRightUI.transform.Find("ActionMenuContainer").gameObject;
        GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;

        ActionMenu.transform.Find("CancelButton").gameObject.SetActive(false);

        InfoMenu.transform.Find("SelectedName").Find("SelectedNameText").gameObject.GetComponent<TextMeshProUGUI>().text = "";
        InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = "";

        ActionMenu.SetActive(false);
        InfoMenu.SetActive(false);
    }

    public void Deselection() {
        if (selectedObject.tag == "Hero") {
            HeroDeselected(selectedObject.GetComponent<HeroControl>());
        }
        else if (selectedObject.tag == "Tile") {
            TileDeselected(selectedObject.GetComponent<Tile>());
        }

        selectedObject = null;
    }

    #endregion

    #region Events

    public void onEndTurnButtonClicked() {
        endTurn();
    }

    public void onMoveButtonClicked() {
        if (selectedObject.GetComponent<HeroControl>() != null) {
            moveHero(selectedObject);
            GameObject InfoMenu = bottomRightUI.transform.Find("InformationMenuContainer").gameObject;
            InfoMenu.transform.Find("SelectedDescription").Find("SelectedDescriptionText").gameObject.GetComponent<TextMeshProUGUI>().text = "Move Hero to selected tile.";
        }
    }

    public void onCancelButtonClicked() {
        cancel();
    }

    #endregion

    #region External Updates

    public void UpdateResourceDisplay(PlayerVariables playerAssets) {
        woodContainer.SetText(playerAssets.wood.ToString());
        foodContainer.SetText(playerAssets.food.ToString());
        joyContainer.SetText(playerAssets.joy.ToString());
    }

    #endregion

}
