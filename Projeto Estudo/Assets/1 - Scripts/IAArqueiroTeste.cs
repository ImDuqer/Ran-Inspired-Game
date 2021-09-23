using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAArqueiroTeste : MonoBehaviour
{
    public float range = 5f;
    public GameObject[] inimigo;
    public GameObject flechaPrefab;
    private GameObject flecha;
    public Transform target;
    public Transform arco;
    public float arcoCD;

    void Start()
    {
        inimigo = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) <= 5)
        {
            Ray raio = new Ray(transform.position, target.position - transform.position);
            Debug.DrawRay(raio.origin, raio.direction * 5, Color.red);

            RaycastHit hit;

            if(Physics.Raycast(raio, out hit, 5))
            {
                if(hit.transform == target)
                {
                    transform.LookAt(target);

                    if(arcoCD < Time.time)
                    {
                        arcoCD = Time.time + 0.5f;

                        Vector3 posicaoAlvo = arco.position;
                        Quaternion rotacaoAlvo = Quaternion.FromToRotation(Vector3.up, arco.forward);

                        flecha = Instantiate(flechaPrefab, posicaoAlvo, rotacaoAlvo);
                        flecha.GetComponent<Rigidbody>().AddForce(arco.forward * 500);
                        Destroy(flecha, 2);
                    }
                }
            }
        }
    }
}
