using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScrollButtonController : MonoBehaviour {


    [SerializeField] Sprite roll;
    [SerializeField] Sprite paper;
    [SerializeField] bool updateButton = true;

    Animator myAnim;
    public bool blocked = false;



    void Start() {
        myAnim = GetComponent<Animator>();
        
        if (blocked) myAnim.SetTrigger("blocked");


        if (updateButton && PlayerPrefs.GetInt("HighWeek") >= 11) {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = paper;
            transform.GetChild(1).GetComponent<Image>().sprite = roll;
            transform.GetChild(2).GetComponent<Image>().sprite = roll;
        }


    }


    

    public void Hover() {
        if (!blocked && !MenuController.desicionMade) myAnim.SetTrigger("hover");
    }

    public void Unhover() {
        if (!blocked && !MenuController.desicionMade) myAnim.SetTrigger("unhover");
    }

    public void Click() {
        if (!blocked && !MenuController.desicionMade) myAnim.SetTrigger("click");
    }

}