using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScrollButtonController : MonoBehaviour {
    Animator myAnim;
    public bool blocked = false;



    void Start() {
        myAnim = GetComponent<Animator>();
        if (blocked) myAnim.SetTrigger("blocked");
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