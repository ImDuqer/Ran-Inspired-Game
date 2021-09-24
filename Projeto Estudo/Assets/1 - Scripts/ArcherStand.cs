using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStand : MonoBehaviour {

    public int archerCost;

    ResourceTracker myRT;

    MeshRenderer myMR;
    Collider myC;
    Transform spawnParent;
    void Start() {
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("DynamicObjects").transform;
    }

    void OnEnable() {
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }
    void OnMouseOver() {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        if (Input.GetMouseButtonDown(0)) {
            if(ResourceTracker.POINTS >= archerCost) {
                ResourceTracker.POINTS -= archerCost;
                myMR.enabled = false;
                myC.enabled = false;
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).SetParent(spawnParent);
                ResourceTracker.CURRENT_POPULATION++;
            }
            else {
                myRT.NotEnoughPoints();
            }
        }
    }
    
    void OnMouseExit() {
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }
}
