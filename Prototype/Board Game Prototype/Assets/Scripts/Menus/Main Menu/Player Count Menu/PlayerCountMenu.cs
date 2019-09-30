using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCountMenu : Menu {

    [SerializeField] GameObject newGameMenu = null;

    public override void onMenuButtonPressed(int index) {
        switch (index) {
            case 0:
                setPlayerCount(1);
                break;
            case 1:
                setPlayerCount(2);
                break;
            case 2:
                setPlayerCount(3);
                break;
            case 3:
                setPlayerCount(4);
                break;
            default:
                setPlayerCount(1);
                break;
        }
        OpenNewGameMenu();
    }

    private void setPlayerCount(int playerCount) {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.setPlayerCount(playerCount);
    }

    private void OpenNewGameMenu() {
        newGameMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}