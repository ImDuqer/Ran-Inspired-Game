using System.Collections;
using UnityEngine;
public abstract class FSMNode : ScriptableObject {
    public enum Status{RUNNING, SUCCESS, FAILURE, NONE}
    public Status status;

    public abstract IEnumerator Run(FSM fsm);

}
