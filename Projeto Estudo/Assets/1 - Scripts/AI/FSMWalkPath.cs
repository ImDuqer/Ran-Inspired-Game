using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSMWalkPath : FSMNode{
    
    int i;
    Transform destination;
    NavMeshAgent myNMA;
    
    public override IEnumerator Run(FSM fsm) {
        i = fsm.GetComponent<EnemyBase>().currentWalkPoint;
        myNMA = fsm.transform.GetComponent<NavMeshAgent>();
        destination = Path.PATH[i];
        status = Status.RUNNING;
        myNMA.SetDestination(destination.position);
        if(!myNMA.pathPending && !myNMA.hasPath) {
            status = Status.SUCCESS;
            fsm.GetComponent<EnemyBase>().currentWalkPoint++;
            yield break;
        }
        if(myNMA.isPathStale) {
            status = Status.FAILURE;   
            yield break;
        }
        
    }
}
