using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODManager : MonoBehaviour {
    static StudioEventEmitter soundtrack;

    public static int currentIntensity;



    static FMODManager instance;
    void Start() {
        instance = this;
        soundtrack = Camera.main.GetComponent<StudioEventEmitter>();
    }


    //soundtrack.SetParameter("Dialog", 1); 0 || 1
    //soundtrack.SetParameter("Intensity", 1); 0 ~ 3
    //soundtrack.SetParameter("Restart Loop", 1); 0 || 1
    static public void DialogueMusicStart() {

        soundtrack.SetParameter("Intensity", 0);
        soundtrack.SetParameter("Dialog", 1);
    }
    static public void DialogueMusicEnd() {
        soundtrack.SetParameter("Dialog", 0); 
        soundtrack.SetParameter("Restart Loop", 1);
    }
    static public void SetDangerLevel(int x) {
        Debug.Log("Intensity: " + x);
        currentIntensity = x;
        soundtrack.SetParameter("Intensity", x);
    }
    static public void PrepPhaseEnd() {
        Debug.Log("Intensity: 0");
        currentIntensity = 0;
        soundtrack.SetParameter("SkipToCombat", 1);
        soundtrack.SetParameter("Intensity", 0);
        soundtrack.SetParameter("Restart Loop", 0);
        instance.StartCoroutine(ResetParam());

    }

    static IEnumerator ResetParam() {

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();


        soundtrack.SetParameter("SkipToCombat", 0);
    }

    public void Update() {
        Debug.Log("currentIntensity: " + currentIntensity);
    }
}
