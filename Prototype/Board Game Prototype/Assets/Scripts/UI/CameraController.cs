using System;
using UnityEngine;

public class CameraController : MonoBehaviour {

    bool ControlsNormal = true;

    public float panSpeed = 10f;
    float panBorderThickness = 10f;
    Vector2 panLimit = new Vector2(10, 10);

    float scrollSpeed = 20f;
    float minScrollY = 2f;
    float maxScrollY = 50f;

    void Update() {
        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness) {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness) {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness) {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness) {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("q")) {
            transform.Rotate(0, 0.75f, 0, 0);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 50f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minScrollY, maxScrollY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
        AdjustPanSpeed(pos);
    }

    public void AdjustPanSpeed(Vector3 pos) {
        if(ControlsNormal == false) {
            panSpeed = -pos.y;
        } else {
            panSpeed = pos.y;
        }
    }

    public void FocusCameraOnPlayer(int id) {
        switch (id) {
            case 0:
                SetControlsNormal();
                break;
            case 1:
                SetControlsReverse();
                break;
            case 2:
                SetControlsNormal();
                break;
            case 3:
                SetControlsReverse();
                break;
            default:
                SetControlsNormal();
                break;
        }
    }

    private void SetControlsNormal() {
        transform.Rotate(0, 0, 0, 0);
        ControlsNormal = true;

    }

    private void SetControlsReverse() {
        transform.Rotate(0, 180f, 0, 0);
        ControlsNormal = false;
    }
}
