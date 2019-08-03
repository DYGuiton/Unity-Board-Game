using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuControl : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public void OpenPauseMenu() {
        gameObject.SetActive(true);
        GameIsPaused = true;
    }

    public void ClosePauseMenu() {
        gameObject.SetActive(false);
        GameIsPaused = false;
    }

    public void OpenMainMenu() {
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene(0);
    }

    public void Quit() {
        Application.Quit();
    }
}
