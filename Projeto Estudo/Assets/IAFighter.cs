using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Adicionei
using UnityEngine.UI;

public class IAFighter : MonoBehaviour {
    

    [HideInInspector] public bool inCombat;

    FSMWalkPath myFP;

    [SerializeField] float speed = 3.5f;
    public float range = 5f;
    Transform target = null;
    //Adicionei
    Slider vidaCastelo;
    int HP = 5;
    int originalHP = 5;
    List<GameObject> lifes = new List<GameObject>();
    EnemySpawner myES;
    NavMeshAgent myNMA;
    Coroutine speedCoroutine;
    void Start() {/*
        myES = GameObject.Find("Dynamic Objects").GetComponent<EnemySpawner>();
        speedCoroutine = null;
        myNMA = GetComponent<NavMeshAgent>();
        myNMA.speed = speed;
        foreach (Transform child in transform) {
            lifes.Add(child.gameObject);
        }
        vidaCastelo = GameObject.Find("VidadoCastelo").GetComponent<Slider>();
        FSMSequence myPath = new FSMSequence();
        myFP = new FSMWalkPath();
        myPath.sequence.Add(myFP);

        FSM fsm = GetComponent<FSM>();
        fsm.root = myPath;

        StartCoroutine(fsm.Begin());*/
    }

    void Update() {
        /*
        if () ;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hitted in hits) {
            if (hitted.transform.CompareTag("Enemy")) {
                target = hitted.transform;
                break;
            }
        }
        */



        if (CardsButton.MOVSPEEDDEBUFF) SpeedDebuff();
    }

    void OnCollisionEnter(Collision other) {/*
        if (other.transform.CompareTag("Arrow")) {

            if (!CardsButton.DAMAGEBUFF) HP -= 1;
            else HP -= 2;
            lifes[lifes.Count - 1].SetActive(false);

            lifes.Remove(lifes[lifes.Count - 1]);
            if (HP <= 0) EnemyReset(true);
            //other.gameObject;
        }*/
    }


    void SpeedDebuff() {
        if (speedCoroutine == null) speedCoroutine = StartCoroutine(SpeedDebuffCoroutine());
    }

    IEnumerator SpeedDebuffCoroutine() {
        myNMA.speed = speed * 0.8f;
        yield return new WaitForSeconds(5);

        myNMA.speed = speed;
        speedCoroutine = null;
        CardsButton.MOVSPEEDDEBUFF = false;
    }

    public void GlobalDamage() {/*
        HP -= 1;

        lifes[lifes.Count - 1].SetActive(false);

        lifes.Remove(lifes[lifes.Count - 1]);
        if (HP <= 0) EnemyReset(true);*/
    }

    void OnTriggerEnter(Collider other) {/*
        if (other.transform.CompareTag("Castle")) {
            EnemyReset(false);
            //Adicionei
            vidaCastelo.value -= 2;
        }*/
    }
    public void EnemyReset(bool points) {/*
        foreach (Transform child in transform) {
            if (!lifes.Contains(child.gameObject)) lifes.Add(child.gameObject);
        }
        if (points) ResourceTracker.POINTS++;
        myES.EnemyPool.Add(this.gameObject);
        transform.SetParent(GameObject.Find("EnemiesPool").transform);
        HP = originalHP;
        gameObject.SetActive(false);*/
    }
}