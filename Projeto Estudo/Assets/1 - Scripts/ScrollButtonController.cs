using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class ScrollButtonController : MonoBehaviour {


    [SerializeField] Sprite roll;
    [SerializeField] Sprite paper;
    [SerializeField] bool updateButton = true;

    Animator myAnim;
    public bool blocked = false;

    StudioEventEmitter som1;
    StudioEventEmitter som2;


    void Start() {

        som1 = Camera.main.transform.GetChild(0).GetComponent<StudioEventEmitter>();
        som2 = Camera.main.transform.GetChild(1).GetComponent<StudioEventEmitter>();
        myAnim = GetComponent<Animator>();
        
        if (blocked) myAnim.SetTrigger("blocked");


        if (updateButton && PlayerPrefs.GetInt("HighWeek") >= 11) {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = paper;
            transform.GetChild(1).GetComponent<Image>().sprite = roll;
            transform.GetChild(2).GetComponent<Image>().sprite = roll;
        }


    }




    public void Hover() {

        if (!blocked && !MenuController.desicionMade) {
            myAnim.SetTrigger("hover");
            som1.Play();
        }
    }

    public void Unhover() {
        if (!blocked && !MenuController.desicionMade) {
            myAnim.SetTrigger("unhover");
            som1.Play();
        }
    }

    public void Click() {
        som1.Play();
        if (!blocked && !MenuController.desicionMade) { 
            myAnim.SetTrigger("click");
            som2.Play();
        }
}

}