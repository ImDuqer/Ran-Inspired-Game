using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConstructionStand : MonoBehaviour {

    public int ConstructionCost;

    ResourceTracker myRT;

    MeshRenderer myMR;
    Collider myC;
    Transform spawnParent;
    bool bought = false;
    TextMeshPro tmp;
    int originalCost;

    void Start() {
        originalCost = ConstructionCost;
        tmp = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<TextMeshPro>();
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable() {
        if (!bought) transform.GetChild(0).transform.gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }
    void OnMouseOver() {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        if (CardsButton.PRICEBUFF) ConstructionCost = originalCost + 1;
        else ConstructionCost = originalCost;
        tmp.text = ConstructionCost.ToString() + " pontos\n+2 população";
        if (Input.GetMouseButtonDown(0)) {
            if (ResourceTracker.POINTS >= ConstructionCost) {
                ResourceTracker.POINTS -= ConstructionCost;
                myMR.enabled = false;
                myC.enabled = false;
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).SetParent(spawnParent);
                ResourceTracker.MAX_POPULATION += 3;
                bought = true;
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
