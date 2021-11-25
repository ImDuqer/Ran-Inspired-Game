using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//Adicionei
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour {

    FSMWalkPath myFP;

    [SerializeField] float speed = 3.5f;

    //Adicionei
    Slider vidaCastelo;
    int HP = 5;
    List<GameObject> lifes = new List<GameObject>();

    NavMeshAgent myNMA;

    void Start() {
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

        StartCoroutine(fsm.Begin());
    }

    void Update() {
        //Debug.Log("Vida: " + HP);
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
        StartCoroutine(SpeedDebuffCoroutine());
    }

    IEnumerator SpeedDebuffCoroutine() {
        myNMA.speed = speed * 0.8f;
        yield return new WaitForSeconds(5);

        myNMA.speed = speed;
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
        }
        if (points) ResourceTracker.POINTS++;
        transform.SetParent(GameObject.Find("EnemiesPool").transform);
        gameObject.SetActive(false);
    }
}


