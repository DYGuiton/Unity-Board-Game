using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex = 0;
    [SerializeField] bool animationPlaying;
    public AudioSource audioSource;
    public delegate void ButtonPressedCallback(int index);
    public ButtonPressedCallback buttonPressedCallback = null;


    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (!animationPlaying) {
            if (Input.GetAxis("Vertical") != 0) {
                if (!keyDown) {
                    if (Input.GetAxis("Vertical") < 0) {
                        if (index < maxIndex) {
                            index++;
                        }
                        else {
                            index = 0;
                        }
                    }
                    else if (Input.GetAxis("Vertical") > 0) {
                        if (index > 0) {
                            index--;
                        }
                        else {
                            index = maxIndex;
                        }
                    }
                    keyDown = true;
                }
            }
            else {
                keyDown = false;
            }
        }
    }

    public void ButtonPressed(int index) {
        buttonPressedCallback(index);
    }

    public void setAnimationPlaying(bool animationPlaying) {
        this.animationPlaying = animationPlaying;
    }

    public void setIndex(int index) {
        if (!animationPlaying) {
            this.index = index;
        }
    }


}
