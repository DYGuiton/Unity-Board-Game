using UnityEngine.EventSystems;
using UnityEngine;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    [SerializeField] MenuButtonController menuButtonController = null;
    [SerializeField] Animator animator = null;
    [SerializeField] MenuAnimatorFunctions animatorFunctions = null;
    [SerializeField] int myIndex = 0;

    void Update() {

        //When the menuButtonController and this button have matching indeces, set the button to its selected state
        if (menuButtonController.index == myIndex) {
            animator.SetBool("selected", true);

            //If the space bar or enter is pressed, allow the button to proceed through its pressed state animations
            //Turn off animation functionality for all other buttons in the MenuButtonController
            if (Input.GetAxis("Submit") == 1) {
                animator.SetBool("pressed", true);
                menuButtonController.setAnimationPlaying(true);
            }

            //When the animator reaches the conclusion of its pressed state animations, 
            //Turn back on animation functionality for all other buttons in the MenuButtonController
            //Trigger the ButtonPressed callback
            else if (animator.GetBool("pressed") && animator.GetCurrentAnimatorStateInfo(0).IsName("menuButtonDeselected")) {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
                menuButtonController.setAnimationPlaying(false);
                menuButtonController.ButtonPressed(myIndex);
            }
        }
        //Set selected to false if the button does not have matching indeces with the menuButtonController
        else {
            animator.SetBool("selected", false);
        }

    }

    //Handling code for pointer button clicking
    //When the pointer enters the button space, set the menuButtonController's indeces to match this buttons
    public void OnPointerEnter(PointerEventData eventData) {
        menuButtonController.setIndex(myIndex);
    }

    //When the button is clicked with the pointer, 
    //the button will proceed through its pressed state animations
    //We turn off animation functionality for all other buttons in the MenuButtonController
    //This is reenabled in the update loop above
    public void OnPointerClick(PointerEventData eventData) {
        animator.SetBool("pressed", true);
        menuButtonController.setAnimationPlaying(true);
    }
}
