using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveCounter : MonoBehaviour {

    TextMeshProUGUI WaveText;
    bool updated;
    void Start() {
        WaveText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        if(EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE) {
            updated = false;
        }
        if (!updated && EnemySpawner.currentGamePhase == GamePhase.REWARD_PHASE) {
            WaveText.text = "Wave " + (EnemySpawner.currentWave + 1);
            updated = true;
        }
    }
}