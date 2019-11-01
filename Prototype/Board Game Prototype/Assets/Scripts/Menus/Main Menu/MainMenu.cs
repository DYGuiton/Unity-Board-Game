using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

    public GameObject gameParametersMenu;
    public GameObject newGameMenuItem;
    public GameObject QuitMenuItem;

    private void Start() {
        gameParametersMenu.GetComponent<GameParametersMenuControl>().backButtonPressed += reactivateMenu;
    }

    private void reactivateMenu() {
        gameObject.SetActive(true);
        gameParametersMenu.SetActive(false);
        newGameMenuItem.GetComponent<Animator>().SetBool("selected", false);
    }

    public override void onMenuButtonPressed(int index) {
        switch (index) {
            case 0:
                newGameMenuItem.GetComponent<Animator>().SetBool("selected", false);
                gameParametersMenu.SetActive(true);
                gameObject.SetActive(false);
                break;
            case 1:
                Quit();
                break;
            default:
                gameParametersMenu.SetActive(true);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Quit() {
        Application.Quit();
    }
}
