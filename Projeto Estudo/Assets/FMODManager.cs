using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODManager : MonoBehaviour {
    StudioEventEmitter soundtrack;
    void Start() {
        soundtrack = Camera.main.GetComponent<StudioEventEmitter>();
    }


    //soundtrack.SetParameter("Dialog", 1); 0 || 1
    //soundtrack.SetParameter("Intensity", 1); 0 ~ 3
    //soundtrack.SetParameter("Restart Loop", 1); 0 || 1
    public void DialogueMusicStart() {

        soundtrack.SetParameter("Dialog", 1);
    }
    public void DialogueMusicEnd() {
        soundtrack.SetParameter("Dialog", 0); 
        soundtrack.SetParameter("Restart Loop", 1);
    }
    public void SetDangerLevel(int x) {
        soundtrack.SetParameter("Intensity", x);
    }
    public void PrepPhaseEnd() {
        soundtrack.SetParameter("Intensity", 0);
        soundtrack.SetParameter("Restart Loop", 0);
    }

    public void Update() {
        
    }
}
