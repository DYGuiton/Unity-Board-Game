using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameMenu : Menu {

    public override void onMenuButtonPressed(int index) {
        switch (index) {
            case 0:
                StartGame(3);
                break;
            case 1:
                StartGame(5);
                break;
            case 2:
                StartGame(10);
                break;
            default:
                StartGame(5);
                break;
        }
    }

    private void StartGame(int MapSize) {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.setMapSize(MapSize);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
