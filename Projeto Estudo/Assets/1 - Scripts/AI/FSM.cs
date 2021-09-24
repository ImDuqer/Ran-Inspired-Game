using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour {
    
    [HideInInspector] public FSMNode root;
    public IEnumerator Begin() {
        while(true) {
            yield return StartCoroutine(root.Run(this));
        }
    }

}
