using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [SerializeField] float panSpeed;
    [SerializeField] float panBorderThickness;
    [SerializeField] Vector2 panLimit;
    [SerializeField] Vector2 xLimits;
    [SerializeField] Vector2 yLimits;
    [SerializeField] Vector2 zLimits;
    [SerializeField] Transform CamHighPos;
    [SerializeField] Transform CamLowPos;
    public static bool shouldPanCamera;
    public static bool inDialogue = false;
    bool zoom = false;
    float panPercent;
    Vector3 pos;
    void Start() {
        shouldPanCamera = true;
        pos = transform.position;
    }

    
    void Update() {
        //Debug.Log("panPercent (" + panPercent + ") = posX (" + Mathf.Abs(pos.x + 1140) + ") * 100 / 70");
        if (shouldPanCamera) {
            if (Input.mousePosition.y >= Screen.height - panBorderThickness) {
                pos.x -= panSpeed * Time.deltaTime;
                float MAXX = Mathf.Abs(xLimits.y) - Mathf.Abs(xLimits.x);
                float CURX = Mathf.Abs(pos.x) - Mathf.Abs(xLimits.x);
                float MAXY = Mathf.Abs(yLimits.y) - Mathf.Abs(yLimits.x);

                pos.y = ((CURX * MAXY) / MAXX) + yLimits.x;
            }

            if (Input.mousePosition.y <= panBorderThickness) {

                pos.x += panSpeed * Time.deltaTime;
                float MAXX = Mathf.Abs(xLimits.y) - Mathf.Abs(xLimits.x);
                float CURX = Mathf.Abs(pos.x) - Mathf.Abs(xLimits.x);
                float MAXY = Mathf.Abs(yLimits.y) - Mathf.Abs(yLimits.x);

                pos.y = ((CURX * MAXY) / MAXX) + yLimits.x;
            }

            if (Input.mousePosition.x >= Screen.width - panBorderThickness) pos.z += panSpeed * Time.deltaTime;

            if (Input.mousePosition.x <= panBorderThickness) pos.z -= panSpeed * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, xLimits.y, xLimits.x);
            pos.y = Mathf.Clamp(pos.y, yLimits.x, yLimits.y);
            pos.z = Mathf.Clamp(pos.z, zLimits.x, zLimits.y);

            transform.position = pos;
        }
        if (Input.GetKeyDown(KeyCode.F12)) {
            ChangeZoom();
        }

        if (zoom) {
            if (shouldPanCamera) {
                CamLowPos.position = transform.position;
                CamLowPos.rotation = transform.rotation;
            }

            shouldPanCamera = false;
            
            Zoom(CamHighPos);
        }
        else if (!shouldPanCamera && !inDialogue) {
            Zoom(CamLowPos);
            if(Vector3.Distance(transform.position, CamLowPos.position) <= 0.08f) {
                transform.position = CamLowPos.position;
                shouldPanCamera = true;
            }
        }
    }

    public void ChangeZoom() {

        zoom = !zoom;
    }

    void Zoom(Transform destination) {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, destination.position, Time.deltaTime * 4), Quaternion.Lerp(transform.rotation, destination.rotation, Time.deltaTime * 4));
    }

}