using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionMode : MonoBehaviour {

    GameObject[] ArcherStands;
    GameObject[] TowerStands;
    GameObject[] ConstructionStands;
    bool onConstruction = false;
    bool CC = false;
    bool AC = false;
    bool TC = false;
    void Awake() {
        ArcherStands = GameObject.FindGameObjectsWithTag("Archer");
        ConstructionStands = GameObject.FindGameObjectsWithTag("Construction");
        TowerStands = GameObject.FindGameObjectsWithTag("Tower");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F8)) {
            ToggleFastForward();
        }
        if (EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE) {
            if (Input.GetKeyDown(KeyCode.F5)) {
                if (CC || TC) {
                    TurnObj(ConstructionStands, false);
                    TurnObj(TowerStands, false);
                    AC = true;
                    CC = false;
                    TC = false;
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
            if (Input.GetKeyDown(KeyCode.F6)) {
                if (AC || TC) {
                    TurnObj(ArcherStands, false);
                    TurnObj(TowerStands, false);
                    AC = false;
                    TC = false;
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
            if (Input.GetKeyDown(KeyCode.F7)) {
                if (AC || CC) {
                    TurnObj(ConstructionStands, false);
                    TurnObj(ArcherStands, false);
                    TC = true;
                    AC = false;
                    CC = false;
                    TurnObj(TowerStands, true);
                    onConstruction = true;
                    return;
                }
                if (onConstruction) {
                    onConstruction = false;
                    TurnObj(TowerStands, false);
                    return;
                }
                CC = true;
                TurnObj(TowerStands, true);
                onConstruction = true;
            }

        }
        else {
            AC = false;
            CC = false;
            TC = false;
            onConstruction = false;
            TurnObj(ArcherStands, false);
            TurnObj(ConstructionStands, false);
            TurnObj(TowerStands, false);
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