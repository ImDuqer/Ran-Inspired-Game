using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanning : MonoBehaviour
{
    [SerializeField] float panSpeed;
    [SerializeField] float panBorderThickness;
    [SerializeField] Vector2 panLimit;
    public static bool shouldPanCamera;
    Vector3 pos;
    void Start() {
        shouldPanCamera = true;
        pos = transform.position;
    }

    
    void Update() {
        if (shouldPanCamera) {
            if (Input.mousePosition.y >= Screen.height - panBorderThickness) pos.x -= panSpeed * Time.deltaTime;

            if (Input.mousePosition.y <= panBorderThickness) pos.x += panSpeed * Time.deltaTime;

            if (Input.mousePosition.x >= Screen.width - panBorderThickness) pos.z += panSpeed * Time.deltaTime;

            if (Input.mousePosition.x <= panBorderThickness) pos.z -= panSpeed * Time.deltaTime;

            //numeros subtraindo temporarios, referente ao centro do mapa na posição global
            pos.x = Mathf.Clamp(pos.x, -1180 - panLimit.x, -1180 + panLimit.x);
            pos.z = Mathf.Clamp(pos.z, 380 - panLimit.y, 380 + panLimit.y);

            transform.position = pos;
        }
    }
    
}