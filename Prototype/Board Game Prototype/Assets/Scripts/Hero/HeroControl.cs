using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : MonoBehaviour {
    public PlayerControl player;
    public Material material;
    public HeroProfile heroProfile { set; get; }
    public float speed = 2;
    public float gravity = 9.8f;

    public Tile currentTile;

    public GameObject destination;
    public bool moving = false;

    public void Update() {
        HandleMovement();

    }

    private void HandleMovement() {
        if (moving) {
            var offset = destination.transform.position - transform.position;
            if (offset.magnitude > 0.1) {
                var controller = gameObject.GetComponentInChildren<CharacterController>();
                float vSpeed = 0;
                offset = offset.normalized * speed;
                offset.y = 0f;
                if (controller.isGrounded) {
                    vSpeed = 0;
                }
                vSpeed -= gravity;
                offset.y = vSpeed;

                controller.Move(offset * Time.deltaTime);
                Debug.Log(offset * Time.deltaTime);
                Debug.Log(gameObject.GetComponentInChildren<CharacterController>().velocity);
            }
            else {
                moving = !moving;
                destination = null;
            }
        }
    }

    public void setMaterial(Material nuMaterial) {
        material = nuMaterial;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = nuMaterial;
    }

    public void MoveToTile(GameObject destinationTile) {
        moving = true;
        destination = destinationTile;
    }
}
