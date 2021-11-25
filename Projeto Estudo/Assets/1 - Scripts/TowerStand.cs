using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerStand : MonoBehaviour
{
    public int TowerCost;

    ResourceTracker myRT;

    MeshRenderer myMR;
    Collider myC;
    Transform spawnParent;
    bool bought = false;
    TextMeshPro tmp;
    int originalCost;

    void Start() {
        originalCost = TowerCost;

        tmp = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshPro>();
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
        transform.GetChild(2).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable() {
        if(!bought) transform.GetChild(0).transform.gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }
    void OnMouseOver() {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        if (CardsButton.PRICEBUFF) TowerCost = originalCost - 1;
        else TowerCost = originalCost;
        tmp.text = TowerCost.ToString() + " pontos\nLocal de arqueiro";
        if (Input.GetMouseButtonDown(0)) {
            if (ResourceTracker.POINTS >= TowerCost) {
                ResourceTracker.POINTS -= TowerCost;
                myMR.enabled = false;
                myC.enabled = false;
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(2).SetParent(spawnParent);
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
