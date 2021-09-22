using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMoveTeste : MonoBehaviour
{
    public Vector3 alvo = new Vector3(1, 1, 1);
    public float speed = 1;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, alvo, Time.deltaTime * speed);
    }
}
