using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] Transform[] path;
    [SerializeField] bool gizmos;

    public static Transform[] PATH;
    void Awake() {
        PATH = path;
        
    }
  
    void OnDrawGizmos() {
        if(gizmos) for(int i = 0; i < path.Length-1; i++) Gizmos.DrawLine(path[i].position, path[i+1].position);
    }
}
