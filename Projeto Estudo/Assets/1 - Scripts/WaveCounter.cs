using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WaveCounter : MonoBehaviour {

    [SerializeField] TextMeshProUGUI WaveText;
    bool updated;
    int number;

    void Update() {
        if(EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE) {
            updated = false;
        }
        if (!updated && EnemySpawner.currentGamePhase == GamePhase.REWARD_PHASE) {
            WaveText.text = "Wave " + (EnemySpawner.ReturnWave() + 1).ToString();
            Debug.Log("currentWave" + EnemySpawner.currentWave);
            updated = true;
        }
    }
}