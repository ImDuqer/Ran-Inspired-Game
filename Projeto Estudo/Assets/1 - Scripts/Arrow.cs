using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    GameObject target;
    Rigidbody myRB;
    IAArqueiroTeste origem;
    void OnEnable() {
        myRB = GetComponent<Rigidbody>();
    }

    public void SetTarget(GameObject target, IAArqueiroTeste origem) {
        this.target = target;
        this.origem = origem;
    }

    void Update() {


        if(target != null) {
            transform.LookAt(target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, target.gameObject.transform.position, 2);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            origem.ReturnArrow(gameObject);
        }
    }
}
