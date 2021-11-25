using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
public enum GamePhase{SETUP_PHASE, ACTION_PHASE, REWARD_PHASE}
public class EnemySpawner : MonoBehaviour {

    public static GamePhase currentGamePhase;
    public static int currentWave = 0;

    public static int currentWeek = 0;

    [SerializeField] TextMeshProUGUI buttonPhase;
    [SerializeField] GameObject waitPhase;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform weekHolder;
    List<Week> weeks = new List<Week>();
    int[] WaveAmmount;
    float timeBetweenSpawns = .5f;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] Animator weekText;

    public GameObject GameplayCanvas;
    public GameObject DialogueCanvas;

    float countdown = 61;

    bool startedSpawning;
    bool aboutToEnd;

    List<GameObject> EnemyPool = new List<GameObject>();

    Transform EnemiesPool;

    int enemiesPoolSize;



    void Awake() {
        foreach (Transform week in weekHolder) {
            weeks.Add(week.gameObject.GetComponent<Week>());
        }
        timeBetweenSpawns = weeks[currentWeek].timeBetweenSpawns;
        WaveAmmount = weeks[currentWeek].waveAmmount; 
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
        //Debug.Log("Wait Phase:" + waitPhase);
        //Debug.Log("currentwave" + currentWave);
        //Debug.Log("waveammount length" +  WaveAmmount.Length);
        if (!startedSpawning && currentGamePhase == GamePhase.ACTION_PHASE) {
            countdown = 61f;
            startedSpawning = true;
            StartCoroutine(Spawning());
        }
        if (Input.GetKeyDown(KeyCode.F9) && currentGamePhase == GamePhase.ACTION_PHASE) {
            StopAllCoroutines();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies) {
                enemy.GetComponent<EnemyBase>().EnemyReset(false);
            }
            aboutToEnd = true;
        }
        if (Input.GetKeyDown(KeyCode.F10) && currentGamePhase == GamePhase.ACTION_PHASE) {
            StopAllCoroutines();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            currentWave = WaveAmmount.Length;
            foreach (GameObject enemy in enemies) {
                enemy.GetComponent<EnemyBase>().EnemyReset(false);
            }
            aboutToEnd = true;
        }

        if (CardsButton.GLOBALDAMAGE) {
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
                enemy.GetComponent<EnemyBase>().GlobalDamage();
            }
            CardsButton.GLOBALDAMAGE = false;
            
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
        if(currentGamePhase == GamePhase.SETUP_PHASE) {
            
            
            countdown -= Time.deltaTime;
            buttonPhase.text = "Começar Onda de Inimigos: " + ((int)countdown).ToString() + " segundos";
            if (countdown <= 0) {
                ActivateActionPhase();
                countdown = 61f;
            }

        }
    }

    public void EndReward() {
        if (currentWave >= WaveAmmount.Length) {
            currentWave = 0;
            StartDialogue();
            currentWeek++;
            CameraPanning.inDialogue = true;
            
        }
    }

    public static int ReturnWave() {
        return currentWave;
    }

    public void StartSetupPhase() {

        timeBetweenSpawns = weeks[currentWeek].timeBetweenSpawns;
        WaveAmmount = weeks[currentWeek].waveAmmount;
        currentGamePhase = GamePhase.SETUP_PHASE;
        CameraPanning.inDialogue = false;
        if (waitPhase.activeSelf) {
            waitPhase.SetActive(false);
            buttonPhase.transform.parent.gameObject.SetActive(true);
        }
        ResourceTracker.POINTS += 5;
    }

    void StartDialogue() {
        FMODManager.DialogueMusicStart();
        CameraPanning.shouldPanCamera = false;
        GameplayCanvas.SetActive(false);
        DialogueCanvas.SetActive(true);
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
        CardsButton.PRICEBUFF = false;
        FMODManager.PrepPhaseEnd();
        currentGamePhase = GamePhase.ACTION_PHASE;
    }
}