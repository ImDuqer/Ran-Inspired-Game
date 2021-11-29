using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateIntensity : MonoBehaviour {

    [SerializeField] int intensityCall = 0;
    List<GameObject> enemies = new List<GameObject>();


    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemies.Add(other.gameObject);
            if (FMODManager.currentIntensity < intensityCall) FMODManager.SetDangerLevel(intensityCall);
        }
    }

    void OnTriggerExit(Collider other) {
        enemies.Remove(other.gameObject);
        if(enemies.Count == 0 && FMODManager.currentIntensity == intensityCall) {
            FMODManager.SetDangerLevel(intensityCall - 1);
        }
    }

}
