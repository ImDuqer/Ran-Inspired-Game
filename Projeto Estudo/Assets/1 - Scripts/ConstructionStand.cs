using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionStand : MonoBehaviour {

    public int ConstructionCost;

    ResourceTracker myRT;

    MeshRenderer myMR;
    Collider myC;
    Transform spawnParent;
    void Start() {
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable() {
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }
    void OnMouseOver() {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        if (Input.GetMouseButtonDown(0)) {
            if (ResourceTracker.POINTS >= ConstructionCost) {
                ResourceTracker.POINTS -= ConstructionCost;
                myMR.enabled = false;
                myC.enabled = false;
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).SetParent(spawnParent);
                ResourceTracker.MAX_POPULATION += 3;
                gameObject.SetActive(false);
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
