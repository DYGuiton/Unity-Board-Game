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
    public List<Tile> destinationPath;
    public int destinationPathIndex = 0;

    public bool moving = false;

    public void Update() {
        HandleMovement();

    }

    private void HandleMovement() {
        if (moving) {
            if (currentTile != destinationPath[0]) {
                var offset = destination.transform.position - transform.position;
                var xOffset = Math.Abs(destination.transform.position.x - transform.position.x);
                var zOffset = Math.Abs(destination.transform.position.z - transform.position.z);
                if (xOffset > 0.1 || zOffset > 0.1) {
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
                    Debug.Log("xOffset = " + xOffset + ", zOffset = " + zOffset + ", offset mag = " + offset.magnitude);
                }
                else {
                    Debug.Log("Success!");
                    Debug.Log("Final Results: xOffset = " + xOffset + ", zOffset = " + zOffset + ", offset mag = " + offset.magnitude);
                    currentTile = destination.GetComponent<Tile>();
                    if (destinationPathIndex > 0) {
                        destinationPathIndex--;
                        destination = destinationPath[destinationPathIndex].gameObject;
                    }
                }
            }
            else {
                moving = false;
                destinationPathIndex = 0;
                destination = null;
                destinationPath = null;
            }
        }
    }

    public void setMaterial(Material nuMaterial) {
        material = nuMaterial;
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = nuMaterial;
    }

    public void MoveHeroAlongPath(List<Tile> movePath) {
        moving = true;
        destinationPathIndex = movePath.Count - 1;
        destinationPath = movePath;
        destination = movePath[movePath.Count - 1].gameObject;
    }
}
