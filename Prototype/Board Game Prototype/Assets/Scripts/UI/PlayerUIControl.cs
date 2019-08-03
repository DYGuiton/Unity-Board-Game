using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    public GameObject playerNameLabel;

    public void Setup(GameObject newPlayer) {
        playerNameLabel.GetComponent<TextMeshProUGUI>().text = newPlayer.name;
        gameObject.SetActive(false);


    }

    public void TurnOn() {
        gameObject.SetActive(true);
        //close all superfluous things
    }
}
