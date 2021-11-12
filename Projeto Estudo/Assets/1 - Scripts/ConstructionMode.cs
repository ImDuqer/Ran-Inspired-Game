using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionMode : MonoBehaviour {

    GameObject[] ArcherStands;
    [SerializeField] GameObject[] ShooterStands;
    [SerializeField] GameObject[] ConstructionStands;
    bool onConstruction = false;
    bool CC = false;
    bool AC = false;
    void Awake() {
        ArcherStands = GameObject.FindGameObjectsWithTag("Archer");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F10)) {
            ToggleFastForward();
        }
        if (EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE) {
            if (Input.GetKeyDown(KeyCode.F8)) {
                if (CC) {
                    TurnObj(ConstructionStands, false);
                    AC = true;
                    CC = false;
                    TurnObj(ArcherStands, true);
                    onConstruction = true;
                    return;
                }
                if (onConstruction) {
                    onConstruction = false;
                    TurnObj(ArcherStands, false);
                    return;
                }
                AC = true;
                TurnObj(ArcherStands, true);
                onConstruction = true;
            }
            if (Input.GetKeyDown(KeyCode.F9)) {
                if (AC) {
                    TurnObj(ArcherStands, false);
                    AC = false;
                    CC = true;
                    TurnObj(ConstructionStands, true);
                    onConstruction = true;
                    return;
                }
                if (onConstruction) {
                    onConstruction = false;
                    TurnObj(ConstructionStands, false);
                    return;
                }
                CC = true;
                TurnObj(ConstructionStands, true);
                onConstruction = true;
            }
        }
        else {
            AC = false;
            CC = false;
            onConstruction = false;
            TurnObj(ArcherStands, false);
            TurnObj(ConstructionStands, false);
        }
    }

    public void ToggleFastForward() {
        Time.timeScale = Time.timeScale == 1 ? 2.5f : 1f;
    }

    void TurnObj(GameObject[] array, bool state){
        foreach (GameObject obj in array) {
            obj.SetActive(state);
        }
    }
}