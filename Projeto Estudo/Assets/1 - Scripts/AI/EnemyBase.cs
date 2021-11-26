using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Adicionei
using UnityEngine.UI;
using FMODUnity;

public class EnemyBase : MonoBehaviour {

    [SerializeField] float range;
    [SerializeField] float attackSpeed;
    float attackSpeedTimer;
    Transform target = null;
    byte i;
    Transform destination;
    bool foundPath = false;
    [SerializeField] float speed = 3.5f;
    [SerializeField] bool gizmos = false;
    Slider vidaCastelo;
    int HP = 5;
    int originalHP = 5;
    List<GameObject> lifes = new List<GameObject>();
    EnemySpawner myES;
    NavMeshAgent myNMA;
    Coroutine speedCoroutine;
    float targetDistance;
    bool walking = true;
    GameObject sideDestination = null;
    bool foundArcher = false;
    bool foundFighter = false;
    StudioEventEmitter mySEE;




    void Start() {
        mySEE = GetComponent<StudioEventEmitter>();
        attackSpeedTimer = 1;
        myES = GameObject.Find("Dynamic Objects").GetComponent<EnemySpawner>();
        speedCoroutine = null;
        myNMA = GetComponent<NavMeshAgent>();
        myNMA.speed = speed;
        foreach (Transform child in transform) {
            lifes.Add(child.gameObject);
        }
        vidaCastelo = GameObject.Find("VidadoCastelo").GetComponent<Slider>();
    }

    void Update() {
        if (CardsButton.MOVSPEEDDEBUFF) SpeedDebuff();


        if (CheckFighters() != null) {
            Debug.Log("Check a fighter!");
            if (targetDistance < 5) Debug.Log("yay?");
            if(targetDistance < range && targetDistance > 3) RunTowards(target);
            else if(targetDistance < range ) {
                Attack();
            }

        }

        else if (CheckArchers() != null) {
            Debug.Log("Check an archer!");
            if (targetDistance < range && targetDistance > 3) RunTowards(target);
            else if (targetDistance < range) {
                Attack();
            }
        }

        else {
            Debug.Log("Followed the path!");
            FollowPath();
            myNMA.SetDestination(destination.position);
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

    void FollowPath() {
        destination = Path.PATH[i];


        if (!walking) {
            foreach (GameObject life in lifes) {
                life.GetComponent<Animator>().SetTrigger("Walk");
            }
            walking = true;
            myNMA.isStopped = false;
        }


        if (myNMA.hasPath) {
            foundPath = true;
        }

        if (!myNMA.hasPath && foundPath) {
            i++;
            destination = null;
            foundPath = false;
            destination = Path.PATH[i];
        }

        if (myNMA.isPathStale) {
        }
    }


    void Attack() {
        Debug.Log("ATTACKING");
        destination = null;
        walking = false;
        myNMA.isStopped = true;
        attackSpeedTimer -= Time.deltaTime;
        if(attackSpeedTimer <= 0) {

            if (CardsButton.ATTACKSPEEDDEBUFF) attackSpeedTimer = attackSpeed * 1.2F;
            else attackSpeedTimer = attackSpeed;
            foreach (GameObject life in lifes) {
                life.GetComponent<Animator>().SetTrigger("Attack");
                mySEE.Play();
                if (sideDestination.GetComponent<IAArqueiroTeste>() != null) sideDestination.GetComponent<IAArqueiroTeste>().TakeDamage();
                else sideDestination.GetComponent<IAFighter>().TakeDamage();
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


    GameObject CheckArchers() {


        //Debug.Log("Tried to check an archer!");
        sideDestination = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Archer")) {
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
    GameObject CheckFighters() {
        sideDestination = null;
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Fighter")) {
                if (/*!hitted.gameObject.GetComponent<IAFighter>().inCombat &&*/ !PathTooLong(hitted.transform.position, 9)) {
                    sideDestination = hitted.gameObject;
                    //sideDestination.GetComponent<IAFighter>().inCombat = true;
                    target = sideDestination.transform;
                    foundFighter = true;
                    targetDistance = Vector3.Distance(hitted.transform.position, transform.position);
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

    void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Arrow")) {

            TakeDamage(1);
        }
    }

    public void TakeDamage(int damage) {
        if (!CardsButton.DAMAGEBUFF) HP -= damage;
        else HP -= damage+1;
        lifes[lifes.Count - 1].SetActive(false);

        lifes.Remove(lifes[lifes.Count - 1]);
        if (HP <= 0) EnemyReset(true);
    }
    void SpeedDebuff() {
        if(speedCoroutine == null) speedCoroutine = StartCoroutine(SpeedDebuffCoroutine());
    }

    IEnumerator SpeedDebuffCoroutine() {
        myNMA.speed = speed * 0.8f;
        yield return new WaitForSeconds(5);

        myNMA.speed = speed;
        speedCoroutine = null;
        CardsButton.MOVSPEEDDEBUFF = false;
    }

    public void GlobalDamage() {
        HP -= 1;
        
        lifes[lifes.Count - 1].SetActive(false);

        lifes.Remove(lifes[lifes.Count - 1]);
        if (HP <= 0) EnemyReset(true);
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Castle")) {
            EnemyReset(false);
            //Adicionei
            vidaCastelo.value -= 2;
        }
    }
    public void EnemyReset(bool points) {
        foundFighter = false;
        foundArcher = false;
        if (sideDestination != null) {
            if (sideDestination.GetComponent<IAArqueiroTeste>() != null) sideDestination.GetComponent<IAArqueiroTeste>().inCombat = false;
            //else sideDestination.GetComponent<IAFighter>().inCombat = false;
        }
        foreach (Transform child in transform) {
            if(!lifes.Contains(child.gameObject)) lifes.Add(child.gameObject);
            child.gameObject.SetActive(true);
        }
        if (points) ResourceTracker.POINTS++;
        myES.EnemyPool.Add(this.gameObject);
        transform.SetParent(GameObject.Find("EnemiesPool").transform);
        HP = originalHP;
        gameObject.SetActive(false);
    }
}


