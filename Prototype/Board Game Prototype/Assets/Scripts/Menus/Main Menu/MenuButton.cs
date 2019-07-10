using UnityEngine.EventSystems;
using UnityEngine;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    [SerializeField] MenuButtonController menuButtonController = null;
    [SerializeField] Animator animator = null;
    [SerializeField] MenuAnimatorFunctions animatorFunctions = null;
    [SerializeField] int thisIndex = 0;

    private bool clicked { get; set; }


    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (menuButtonController.index == thisIndex) {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1) {
                animator.SetBool("pressed", true);
                clicked = true;
            }
            else if (clicked == true) {
                animatorFunctions.disableOnce = true;
                animator.SetBool("pressed", true);
                menuButtonController.ButtonPressed(thisIndex);
                clicked = false;
            }
        }
        else {
            animator.SetBool("selected", false);
            animator.SetBool("pressed", false);
        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
        menuButtonController.index = thisIndex;
    }

    public void OnPointerClick(PointerEventData eventData) {
        animator.SetBool("pressed", true);
        clicked = true;
    }
}
