using UnityEngine;

public abstract class Menu : MonoBehaviour {
    [SerializeField] public MenuButtonController menuButtonController = null;

    private void Awake() {
        menuButtonController.buttonPressedCallback += onMenuButtonPressed;
    }

    public abstract void onMenuButtonPressed(int index);



}
