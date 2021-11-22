using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStand : MonoBehaviour {

    public int archerCost;

    ResourceTracker myRT;

    [SerializeField] MeshRenderer myMR;
    [SerializeField] Collider myC;
    Transform spawnParent;
    void Start() {
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
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
            if (ResourceTracker.MAX_POPULATION >= ResourceTracker.CURRENT_POPULATION + 1) {
                if (ResourceTracker.POINTS >= archerCost) {
                    ResourceTracker.POINTS -= archerCost;
                    myMR.enabled = false;
                    myC.enabled = false;
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(2).SetParent(spawnParent);
                    ResourceTracker.CURRENT_POPULATION++;
                    gameObject.SetActive(false);
                }
                else {
                    myRT.NotEnoughPoints();
                }
            }
            else myRT.NotEnoughSpace();
        }
    }
    
    void OnMouseExit() {
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }
}
