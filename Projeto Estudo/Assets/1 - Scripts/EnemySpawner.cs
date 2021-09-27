using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GamePhase{SETUP_PHASE, ACTION_PHASE, REWARD_PHASE}



public class EnemySpawner : MonoBehaviour {
    public static GamePhase currentGamePhase;
    [SerializeField] Transform startPosition;
    public static int currentWave = 0;
    [SerializeField] int[] WaveAmmount;
    [SerializeField] float timeBetweenSpawns = .5f;
    List<GameObject> EnemyPool = new List<GameObject>();
    bool startedSpawning;
    bool aboutToEnd;
    Transform EnemiesPool;
    [SerializeField] GameObject rewardPanel;
    int enemiesPoolSize;
    
    void Awake() {
        EnemiesPool = GameObject.Find("EnemiesPool").transform;
        currentGamePhase = GamePhase.SETUP_PHASE;
        foreach (Transform enemy in EnemiesPool) {
            EnemyPool.Add(enemy.gameObject);
        }
        enemiesPoolSize = EnemyPool.Count;
    }

    void Update() {
        if(!startedSpawning && currentGamePhase == GamePhase.ACTION_PHASE) {
            startedSpawning = true;
            StartCoroutine(Spawning());
        }
        if (aboutToEnd && EnemiesPool.childCount >= enemiesPoolSize) {
            currentWave++;
            currentGamePhase = GamePhase.REWARD_PHASE;
            rewardPanel.SetActive(true);
            aboutToEnd = false;
        }
        if(currentGamePhase == GamePhase.REWARD_PHASE) {
            startedSpawning = false;
        }
    }

    public void EndReward() {
        currentGamePhase = GamePhase.SETUP_PHASE;
        ResourceTracker.POINTS += 5;
    }

    IEnumerator Spawning() {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < WaveAmmount[currentWave]; i++) {
            GameObject tempEnemy = EnemyPool[0];
            tempEnemy.SetActive(true);
            tempEnemy.transform.SetParent(transform);
            EnemyPool.Remove(tempEnemy);
            tempEnemy.GetComponent<NavMeshAgent>().Warp(startPosition.position);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
        aboutToEnd = true;
    } 

    public static void ActivateActionPhase() {
        currentGamePhase = GamePhase.ACTION_PHASE;
    }

}