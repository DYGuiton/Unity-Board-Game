using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : BoardObject {
    public HeroProfile heroProfile;
    public Tile currentTile;

    public GameObject destination;
    public List<Tile> destinationPath;
    public int destinationPathIndex = 0;

    public bool moving = false;
    public bool isInteractable = true;

    public float speed = 2;
    public float gravity = 9.8f;

    public void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (moving) {
            isInteractable = false;
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
                }
                else {
                    currentTile = destination.GetComponent<Tile>();
                    if (destinationPathIndex > 0) {
                        destinationPathIndex--;
                        destination = destinationPath[destinationPathIndex].gameObject;
                    }
                }
            }
            else {
                moving = false;
                isInteractable = true;
                destinationPathIndex = 0;
                destination = null;
                destinationPath = null;
            }
        }
    }

    public void setMaterial(Material material) {
        originalMaterial = new Material(material);
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = material;

    }

    public void MoveHeroAlongPath(List<Tile> movePath) {
        moving = true;
        destinationPathIndex = movePath.Count - 1;
        destinationPath = movePath;
        destination = movePath[movePath.Count - 1].gameObject;
    }

    public override bool Highlight() {
        if (isInteractable) {
            Material tileMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
            tileMaterial.shader = Shader.Find("Outlined/UltimateOutline");
            tileMaterial.SetColor("_FirstOutlineColor", new Color(0, 255, 255, 1));
            tileMaterial.SetFloat("_FirstOutlineWidth", 0.1f);
            tileMaterial.SetColor("_SecondOutlineColor", new Color(0, 255, 255, 0));
            tileMaterial.SetFloat("_SecondOutlineWidth", 0.0f);
            return true;
        }
        return false;
    }

    public override bool Highlight(Material highlightMaterial) {
        if (isInteractable) {
            Material currentTileMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
            currentTileMaterial = highlightMaterial;
            return true;
        }
        return false;
    }

    public override void Unhighlight() {
        gameObject.GetComponentInChildren<MeshRenderer>().material = originalMaterial;
    }
}
