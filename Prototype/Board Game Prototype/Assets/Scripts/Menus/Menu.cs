using UnityEngine;

public abstract class Menu : MonoBehaviour {
    [SerializeField] public MenuButtonController menuButtonController = null;

    private void Start() {
        menuButtonController.buttonPressedCallback += onMenuButtonPressed;
    }

    public abstract void onMenuButtonPressed(int index);



}
