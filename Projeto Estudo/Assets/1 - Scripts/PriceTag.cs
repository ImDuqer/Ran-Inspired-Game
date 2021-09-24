using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceTag : MonoBehaviour {

    Transform camTransform;
    void Start() {
        camTransform = Camera.main.transform;
    }
    void Update() {
        transform.LookAt(2 * transform.position - camTransform.position);
    }
}
