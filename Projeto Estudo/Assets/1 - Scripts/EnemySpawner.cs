using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;
public enum GamePhase{SETUP_PHASE, ACTION_PHASE, REWARD_PHASE}
public class EnemySpawner : MonoBehaviour {

    public static GamePhase currentGamePhase;
    public static int currentWave = 0;

    public static int currentWeek;

    [SerializeField] TextMeshProUGUI buttonPhase;
    [SerializeField] GameObject waitPhase;
    [SerializeField] Transform startPosition;
    [SerializeField] Transform weekHolder;
    List<Week> weeks = new List<Week>();
    int[] WaveAmmount;
    float timeBetweenSpawns = .5f;
    [SerializeField] GameObject rewardPanel;
    [SerializeField] TextMeshProUGUI weekText;
    [SerializeField] GameObject defeat;
    [SerializeField] Slider health;
    public GameObject GameplayCanvas;
    public GameObject DialogueCanvas;

    float countdown = 61;

    bool startedSpawning;
    bool aboutToEnd;
    [SerializeField] GameObject byobu2Panel;
    public List<GameObject> EnemyPool = new List<GameObject>();

    Transform EnemiesPool;

    int enemiesPoolSize;



    void Awake() {
        currentWeek = PlayerPrefs.GetInt("CurrentWeek");
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
        if (Input.GetKeyDown(KeyCode.F1)) {

            PlayerPrefs.SetInt("CurrentWeek", 0);
            currentWeek = 0;
        }
        if (Input.GetKeyDown(KeyCode.F3) || (health.value <= 0)) {

            PlayerPrefs.SetInt("CurrentWeek", 0);
            GameplayCanvas.SetActive(false);
            defeat.SetActive(true);

        }
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
            //Debug.Log("enemies: " + enemies[0]);
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
            if (currentWeek != 7) rewardPanel.SetActive(true);
            else byobu2Panel.SetActive(true);
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
            if (currentWeek < 8) {
                StartDialogue();
                CameraPanning.inDialogue = true;
            }/*
            else if (currentWeek == 8) {

                StartDialogue();
                CameraPanning.inDialogue = true;
            }*/
            else {

                weekText.text = "Semana " + (EnemySpawner.currentWeek + 1);
                weekText.transform.parent.GetComponent<Animator>().SetTrigger("showup");
                CameraPanning.shouldPanCamera = true;
            }
            currentWeek++;
            PlayerPrefs.SetInt("CurrentWeek", currentWeek);
            if (currentWeek > PlayerPrefs.GetInt("HighWeek")) PlayerPrefs.SetInt("HighWeek", currentWeek);
            
            
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