using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour {

    FSMWalkPath myFP;
    int HP = 3;
    void Start() {
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
            HP -= 1;
            if (HP <= 0) EnemyReset(true);
            //other.gameObject;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Castle")) {
            EnemyReset(false);
        }
    }
    void EnemyReset(bool points) {
        if(points) ResourceTracker.POINTS++;
        transform.SetParent(GameObject.Find("EnemiesPool").transform);
        gameObject.SetActive(false);
    }
}


