using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [SerializeField] float panSpeed;
    [SerializeField] float panBorderThickness;
    [SerializeField] Vector2 panLimit;
    [SerializeField] Transform CamHighPos;
    [SerializeField] Transform CamLowPos;
    public static bool shouldPanCamera;
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
                CorrectHeight();
            }

            if (Input.mousePosition.y <= panBorderThickness) {
                pos.x += panSpeed * Time.deltaTime;
                CorrectHeight();
            }

            if (Input.mousePosition.x >= Screen.width - panBorderThickness) pos.z += panSpeed * Time.deltaTime;

            if (Input.mousePosition.x <= panBorderThickness) pos.z -= panSpeed * Time.deltaTime;

            //numeros subtraindo temporarios, referente ao centro do mapa na posição global
            pos.x = Mathf.Clamp(pos.x, -1175 - panLimit.x, -1175 + panLimit.x);
            pos.z = Mathf.Clamp(pos.z, 380 - panLimit.y, 380 + panLimit.y);

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
        else if (!shouldPanCamera) {
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
    void CorrectHeight() {

        panPercent = (Mathf.Abs(pos.x + 1140) * 100) / 70;
        panPercent = Mathf.Clamp(panPercent, 1, 100);
        pos.y = (panPercent * 31 / 100) + 27;
    }
}