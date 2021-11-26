using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Adicionei
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour {

    float range;
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
    void Start() {
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
            if(targetDistance < range && targetDistance > range / 4) RunTowards(target);
            else if(targetDistance < range) {
                Attack();
            }

        }

        else if (CheckArchers() != null) {

        }

        else {

        }


    }

    void Attack() {

    }

    void RunTowards(Transform t) {

        destination = Path.PATH[i];


        myNMA.SetDestination(destination.position);

        if (myNMA.hasPath) {
            foundPath = true;
        }

        if (!myNMA.hasPath && foundPath) {
            i++;
            destination = null;
            foundPath = false;
            //destination = Path.PATH[i];
        }

        if (myNMA.isPathStale) {
        }

    }


    GameObject CheckArchers() {


        GameObject sideDestination = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Archer")) {
                sideDestination = hitted.gameObject;
                target = sideDestination.transform;
                break;
            }
        }

        return sideDestination;
    }
    GameObject CheckFighters() {
        GameObject sideDestination = null;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Fighter")) {
                sideDestination = hitted.gameObject;
                target = sideDestination.transform;
                break;
            }
        }
        return sideDestination;
    }
    void WalkToCastle() {

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        if (gizmos) Gizmos.DrawWireSphere(transform.position, range);
    }

    void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Arrow")) {

            if (!CardsButton.DAMAGEBUFF) HP -= 1;
            else HP -= 2;
            lifes[lifes.Count - 1].SetActive(false);

            lifes.Remove(lifes[lifes.Count - 1]);
            if (HP <= 0) EnemyReset(true);
            //other.gameObject;
        }
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


