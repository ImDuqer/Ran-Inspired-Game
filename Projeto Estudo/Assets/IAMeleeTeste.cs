/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAMeleeTeste : MonoBehaviour
{
    Transform alvo = null;
    [SerializeField] Material[] materiais;
    [SerializeField] bool gizmos;
    MeshRenderer meshUnidade;
    bool checarInimigo;
    public float range = 5f;
    public float armaCD;
    float tempoPassou;
    void Start()
    {
        meshUnidade = GetComponent<MeshRenderer>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(gizmos)Gizmos.DrawWireSphere(transform.position, range);
    }


    void Update() {
        ChecarAlvo();
    }

    void ChecarAlvo() {
        if(alvo == null){
            meshUnidade.material = materiais[0];
            Collider[] hits = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hitted in hits) {
                if (hitted.transform.CompareTag("Enemy")) {
                    alvo = hitted.transform;
                    break;
                }
            }
        }
    }

    void AlvoEncontrado(){
        if (alvo != null) {
            
            if(Vector3.Distance(alvo.position, transform.position) > range) alvo = null;
            meshUnidade.material = materiais[1];

            if (tempoPassou >= armaCD) {
                tempoPassou = 0;


            }

        }
    }

}*/
