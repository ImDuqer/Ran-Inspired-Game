using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMWalkPath : FSMNode{
    
    byte i;
    Transform destination;
    bool foundPath = false;
    NavMeshAgent myNMA;
    void Start() {
        i = 0;
    }

    public override IEnumerator Run(FSM fsm) {

        //Debug.Log("currentWalkPoint: " + fsm.GetComponent<EnemyBase>().currentWalkPoint);
        //Debug.Log("i: " + i);
        
        myNMA = fsm.transform.GetComponent<NavMeshAgent>();
        //Debug.Log("hasPath" + myNMA.hasPath);
        //Debug.Log("PATH: " + Path.PATH);
        status = Status.RUNNING;
        destination = Path.PATH[i];
        myNMA.SetDestination(destination.position);

        if (myNMA.hasPath) {
            foundPath = true;
        }

        if (!myNMA.hasPath && foundPath) {
            status = Status.SUCCESS;
            i++;
            destination = null;
            foundPath = false;
            //destination = Path.PATH[i];
            yield break;
        }

        if(myNMA.isPathStale) {
            status = Status.FAILURE;   
            yield break;
        }

    }
}