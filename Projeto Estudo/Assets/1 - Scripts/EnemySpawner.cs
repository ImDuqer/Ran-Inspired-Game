using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum GamePhase{SETUP_PHASE, ACTION_PHASE, REWARD_PHASE}
public class EnemySpawner : MonoBehaviour {

    public static GamePhase currentGamePhase;
    public static int currentWave = 0;

    public static int currentWeek = 0;

    [SerializeField] Transform startPosition;
    [SerializeField] int[] WaveAmmount;
    [SerializeField] float timeBetweenSpawns = .5f;
    [SerializeField] GameObject rewardPanel;

    public GameObject GameplayCanvas;
    public GameObject DialogueCanvas;

    bool startedSpawning;
    bool aboutToEnd;

    List<GameObject> EnemyPool = new List<GameObject>();

    Transform EnemiesPool;

    int enemiesPoolSize;



    void Awake() {
        EnemiesPool = GameObject.Find("EnemiesPool").transform;
        currentGamePhase = GamePhase.SETUP_PHASE;
        foreach (Transform enemy in EnemiesPool) {
            EnemyPool.Add(enemy.gameObject);
        }
        enemiesPoolSize = EnemyPool.Count;
    }

    void EndTutorial() {

    }

    void Update() {
        //Debug.Log("currentwave" + currentWave);
        //Debug.Log("waveammount length" +  WaveAmmount.Length);
        if (!startedSpawning && currentGamePhase == GamePhase.ACTION_PHASE) {
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
            EndReward();
        }
    }

    public void EndReward() {
        currentGamePhase = GamePhase.SETUP_PHASE;
        if (currentWave == WaveAmmount.Length) {
            StartDialogue();
            currentWave = 0;
        }
        ResourceTracker.POINTS += 5;
    }

    void StartDialogue() {
        GameplayCanvas.SetActive(false);
        DialogueCanvas.SetActive(true);
        CameraPanning.shouldPanCamera = false;
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