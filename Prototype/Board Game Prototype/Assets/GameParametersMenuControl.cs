using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameParametersMenuControl : Menu {

    private int playerCount = 1;
    private int mapSize = 3;
    private string[] playerNames;

    public GameObject PlayerRegisterFields;
    public PlayerRegisterFieldControl[] playerRegisterFieldControls = new PlayerRegisterFieldControl[4];

    public delegate void BackButtonCallback();
    public BackButtonCallback backButtonPressed = null;


    void Start() {
        gameObject.SetActive(false);
    }


    public override void onMenuButtonPressed(int index) {

        switch (index) {
            case 0:
                if (!CheckAllPlayersRegistered())
                    return;
                StartGame();
                break;
            case 1:
                backButtonPressed();
                break;
            default:
                if (!CheckAllPlayersRegistered())
                    return;
                StartGame();
                break;
        }
    }

    private bool CheckAllPlayersRegistered() {

        for (int i = 0; i < playerRegisterFieldControls.Length; i++) {
            if (playerRegisterFieldControls[i].gameObject.activeSelf) {
                if (playerRegisterFieldControls[i].playerNameInput.text.Length == 0) {
                    return false;
                }
                if (!playerRegisterFieldControls[i].ReadyToggle.isOn) {
                    return false;
                }
            }
        }
        return true;

    }

    private void StartGame() {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.setMapSize(mapSize);
        gameManager.setPlayerCount(playerCount);
        gameManager.setPlayerNames(CreatePlayerNamesList());

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private string[] CreatePlayerNamesList() {
        playerNames = new string[playerCount];
        for (int i = 0; i < playerRegisterFieldControls.Length; i++) {
            if (playerRegisterFieldControls[i].gameObject.activeSelf) {
                playerNames[i] = playerRegisterFieldControls[i].playerNameInput.text;
            }
        }
        return playerNames;
    }

    public void OnPlayerCountChanged(TMP_Dropdown dropdown) {
        switch (dropdown.value) {
            case 0:
                playerCount = 1;
                break;
            case 1:
                playerCount = 2;
                break;
            case 2:
                playerCount = 3;
                break;
            case 3:
                playerCount = 4;
                break;
            default:
                playerCount = 1;
                break;
        }

        for (int i = 0; i < playerRegisterFieldControls.Length; i++) {
            if (i <= playerCount - 1) {
                playerRegisterFieldControls[i].gameObject.SetActive(true);
            }
            else {
                playerRegisterFieldControls[i].gameObject.SetActive(false);
            }
        }

    }

    public void onMapSizeChanged(TMP_Dropdown dropdown) {
        switch (dropdown.value) {
            case 0:
                mapSize = 3;
                break;
            case 1:
                mapSize = 5;
                break;
            case 2:
                mapSize = 10;
                break;
            default:
                mapSize = 5;
                break;
        }
    }
}
