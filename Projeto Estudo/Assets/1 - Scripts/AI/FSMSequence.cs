using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMSequence : FSMNode {

    public List<FSMNode> sequence = new List<FSMNode>();

    public override IEnumerator Run(FSM fsm)
    {
        status = Status.RUNNING;
        foreach(FSMNode node in sequence){
            yield return fsm.StartCoroutine(node.Run(fsm));
            if(node.status == Status.FAILURE){
                status = Status.FAILURE;
                break;
            }
        }
        if(status == Status.RUNNING) status = Status.SUCCESS;
    }

}
