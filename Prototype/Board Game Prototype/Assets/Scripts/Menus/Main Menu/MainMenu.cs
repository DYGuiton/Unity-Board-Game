using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

    [SerializeField] GameObject playerCountMenu = null;

    public override void onMenuButtonPressed(int index) {
        switch (index) {
            case 0:
                playerCountMenu.SetActive(true);
                gameObject.SetActive(false);
                break;
            case 1:
                Quit();
                break;
            default:
                playerCountMenu.SetActive(true);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Quit() {
        Application.Quit();
    }
}
