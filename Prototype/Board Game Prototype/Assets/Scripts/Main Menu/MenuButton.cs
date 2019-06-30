using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour {
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] MenuAnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;


    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (menuButtonController.index == thisIndex) {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1) {
                animator.SetBool("pressed", true);
            }
            else if (animator.GetBool("pressed")) {
                animatorFunctions.disableOnce = true;
            }
        }
        else {
            animator.SetBool("selected", false);
        }

    }
}
