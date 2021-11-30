using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SamuraiStand : MonoBehaviour {

    public int SamuraiCost;

    ResourceTracker myRT;

    MeshRenderer myMR;
    Collider myC;
    Transform spawnParent;
    [HideInInspector] public bool bought = false;
    TextMeshPro tmp;
    int originalCost;

    bool loaded = false;
    void Start() {
        originalCost = SamuraiCost;
        tmp = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).GetComponent<TextMeshPro>();
        myMR = GetComponent<MeshRenderer>();
        myC = GetComponent<Collider>();
        myRT = GameObject.Find("ResourcesBox").GetComponent<ResourceTracker>();
        spawnParent = GameObject.Find("Dynamic Objects").transform;
        transform.GetChild(2).gameObject.SetActive(false);
        if (!bought) gameObject.SetActive(false);
    }

    private void Update() {

        if (bought && !loaded) {
            loaded = true;
            myMR.enabled = false;
            myC.enabled = false;
            GameObject obj = transform.GetChild(2).gameObject;
            obj.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            obj.GetComponent<IAFighter>().originalParent = gameObject;
            obj.transform.SetParent(spawnParent);

            gameObject.SetActive(false);
        }

        loaded = true;
    }

    void OnEnable() {
        if (transform.childCount > 2) transform.GetChild(2).gameObject.SetActive(false);
        if (!bought) transform.GetChild(0).transform.gameObject.SetActive(false);
        else gameObject.SetActive(false);
    }



    void OnMouseOver() {
        Debug.Log("aaaaaaaaaaaaaaaaaaah");
        transform.GetChild(0).gameObject.SetActive(true);
        if (CardsButton.PRICEBUFF) SamuraiCost = originalCost - 1;
        else SamuraiCost = originalCost;
        tmp.text = SamuraiCost.ToString() + " pontos\n1 popula��o";
        if (Input.GetMouseButtonDown(0)) {
            if (ResourceTracker.MAX_POPULATION >= ResourceTracker.CURRENT_POPULATION + 1) {
                if (ResourceTracker.POINTS >= SamuraiCost) {
                    ResourceTracker.POINTS -= SamuraiCost;
                    myMR.enabled = false;
                    myC.enabled = false;
                    GameObject obj = transform.GetChild(2).gameObject;
                    obj.SetActive(true);
                    transform.GetChild(0).gameObject.SetActive(false);
                    obj.GetComponent<IAFighter>().originalParent = gameObject;
                    obj.transform.SetParent(spawnParent);
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

