using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArcherStand : MonoBehaviour {

    public int archerCost;

    ResourceTracker myRT;

    [SerializeField] MeshRenderer myMR;
    [SerializeField] Collider myC;
    Transform spawnParent;
    [HideInInspector] public bool bought = false;
    TextMeshPro tmp;
    int originalCost;

    void Start() {
        originalCost = archerCost;
        tmp = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<TextMeshPro>();
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable() {

        if (transform.childCount > 2) transform.GetChild(2).gameObject.SetActive(false);
        if (!bought) transform.GetChild(0).gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }
    void OnMouseOver() {
        transform.GetChild(0).gameObject.SetActive(true);
        //Debug.Log("Child: " + transform.GetChild(0));
        if (CardsButton.PRICEBUFF) archerCost = originalCost - 1;
        else archerCost = originalCost;
        //Debug.Log(tmp.text);
        tmp.text = archerCost.ToString() + " pontos\n1 população";
        if (Input.GetMouseButtonDown(0)) {
            if (ResourceTracker.MAX_POPULATION >= ResourceTracker.CURRENT_POPULATION + 1) {
                if (ResourceTracker.POINTS >= archerCost) {
                    ResourceTracker.POINTS -= archerCost;
                    myMR.enabled = false;
                    myC.enabled = false;
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    transform.GetChild(2).GetComponent<IAArqueiroTeste>().originalParent = gameObject;
                    transform.GetChild(2).SetParent(spawnParent);
                    ResourceTracker.CURRENT_POPULATION++;
                    bought = true;
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