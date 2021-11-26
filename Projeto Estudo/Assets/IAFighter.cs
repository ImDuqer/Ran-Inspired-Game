using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Adicionei
using UnityEngine.UI;
using FMODUnity;

public class IAFighter : MonoBehaviour {



    [SerializeField] float range;
    [SerializeField] float attackSpeed;
    Transform initialPos;
    bool returned = false;
    float attackSpeedTimer;
    Transform target = null;
    byte i;
    Transform destination;
    bool foundPath = false;
    [SerializeField] float speed = 3.5f;
    [SerializeField] bool gizmos = false;
    Slider vidaCastelo;
    int HP = 3;
    int originalHP = 3;
    List<GameObject> lifes = new List<GameObject>();
    EnemySpawner myES;
    NavMeshAgent myNMA;
    Coroutine speedCoroutine;
    float targetDistance;
    bool walking = false;
    GameObject sideDestination = null;
    bool foundArcher = false;
    bool foundFighter = false;
    StudioEventEmitter mySEE;

    public GameObject originalParent;



    void Start() {
        initialPos = transform;
        mySEE = GetComponent<StudioEventEmitter>();
        attackSpeedTimer = 0;
        myES = GameObject.Find("Dynamic Objects").GetComponent<EnemySpawner>();
        speedCoroutine = null;
        myNMA = GetComponent<NavMeshAgent>();
        myNMA.speed = speed;
        foreach (Transform child in transform) {
            lifes.Add(child.gameObject);
            child.GetComponent<Animator>().SetTrigger("Idle");
        }
    }


    void SpeedBuff() {
        if (speedCoroutine == null) speedCoroutine = StartCoroutine(SpeedBuffCoroutine());
    }

    IEnumerator SpeedBuffCoroutine() {
        myNMA.speed = speed * 1.2f;
        yield return new WaitForSeconds(5);

        myNMA.speed = speed;
        speedCoroutine = null;
        CardsButton.MOVSPEEDBUFF = false;
    }

    void Update() {
        if (CardsButton.MOVSPEEDBUFF) SpeedBuff();
        if (!walking) {
            foreach (GameObject life in lifes) {
                life.GetComponent<Animator>().SetTrigger("Idle");
            }
        }

        if (CheckEnemies() != null) {
            //Debug.Log("Check a fighter!");
            //if (targetDistance < 5) Debug.Log("yay?");
            if (targetDistance < range && targetDistance > 3) RunTowards(target);
            else if (targetDistance < range) {
                Attack();
            }

        }
        if(EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE && !returned) {
            returned = true;
            myNMA.Warp(initialPos.position);

        }



    }


    bool PathTooLong(Vector3 targetPosition, float maxDistance) {



        NavMeshPath path = new NavMeshPath();
        if (myNMA.enabled)
            myNMA.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;

        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++) {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0;

        for (int i = 0; i < allWayPoints.Length - 1; i++) {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }


        //Debug.Log("pathLenght" + pathLength);
        if (pathLength > maxDistance) return true;
        else return false;

    }



    void Attack() {
        //Debug.Log("ATTACKING");
        destination = null;
        walking = false;
        myNMA.isStopped = true;
        attackSpeedTimer -= Time.deltaTime;
        if (attackSpeedTimer <= 0) {

            if (CardsButton.ATTACKSPEEDBUFF) attackSpeedTimer = attackSpeed * 0.8F;
            else attackSpeedTimer = attackSpeed;
            foreach (GameObject life in lifes) {
                life.GetComponent<Animator>().SetTrigger("Attack");
                mySEE.Play();
                sideDestination.GetComponent<EnemyBase>().TakeDamage(2);
                
            }
        }
        if (sideDestination == null) {
            targetDistance = Mathf.Infinity;
        }
    }

    void RunTowards(Transform t) {
        if (!walking) {
            foreach (GameObject life in lifes) {
                life.GetComponent<Animator>().SetTrigger("Walk");
            }
            walking = true;
            myNMA.isStopped = false;
        }
        destination = t;

        myNMA.SetDestination(destination.position);



    }


    GameObject CheckEnemies() {


        //Debug.Log("Tried to check an archer!");
        sideDestination = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Enemy")) {
                if (/*!hitted.gameObject.GetComponent<IAArqueiroTeste>().inCombat &&*/ !PathTooLong(hitted.transform.position, 9)) {
                    sideDestination = hitted.gameObject;
                    //sideDestination.GetComponent<IAArqueiroTeste>().inCombat = true;
                    target = sideDestination.transform;
                    foundArcher = true;
                    targetDistance = Vector3.Distance(hitted.transform.position, transform.position);
                    //Debug.Log("targetDistance" + targetDistance);
                    break;
                }
            }
        }

        return sideDestination;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (gizmos) Gizmos.DrawWireSphere(transform.position, range);
    }


    
    public void TakeDamage() {
        if (CardsButton.DAMAGEDEBUFF) {
            HP -= 1;
        }
        else HP -= 2;

        if (HP <= 0) EnemyReset();
    }


    public void EnemyReset() {
        originalParent.GetComponent<ArcherStand>().bought = false;
        transform.SetParent(originalParent.transform);
        gameObject.SetActive(false);
        HP = 3;
    }
}