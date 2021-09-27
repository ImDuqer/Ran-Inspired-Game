using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAArqueiroTeste : MonoBehaviour
{
    public float range = 5f;
    public GameObject flechaPrefab;
    private GameObject arrow; 
    Transform target = null;
    public Transform arco;
    public float arcoCD;
    float tempoPassado;
    [SerializeField] bool gizmos;
    [SerializeField] bool debugTarget;
    [SerializeField] Material[] myMaterials;
    MeshRenderer myMR;
    public static List<GameObject> ArrowPool = new List<GameObject>();
    public static bool filledPool;

    bool enemyCheck;

    Transform dynamicObjects;



    void OnEnable() {
        dynamicObjects = GameObject.Find("DynamicObjects").transform;
        if (!filledPool) {
            filledPool = true;
            foreach (Transform arrow in GameObject.Find("ArrowPool").transform) {
                ArrowPool.Add(arrow.gameObject);
            }
        }
        myMR = GetComponent<MeshRenderer>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(gizmos)Gizmos.DrawWireSphere(transform.position, range);
    }

    void Update() {
        if (debugTarget) Debug.Log(target);
        NullTargetCheck();
        FoundTargetCheck();
    }

    void NullTargetCheck() {
        if (target == null) {
            myMR.material = myMaterials[0];
            Collider[] hits = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hitted in hits) {
                if (hitted.transform.CompareTag("Enemy")) {
                    target = hitted.transform;
                    break;
                }
            }
        }
    }

    void FoundTargetCheck() {
        if (target != null) {

            


            if (Vector3.Distance(target.position, transform.position) > range) target = null;
            myMR.material = myMaterials[1];

            if (tempoPassado >= arcoCD) {
                tempoPassado = 0;

                GameObject tempArrow = ArrowPool[0];
                tempArrow.SetActive(true);
                tempArrow.transform.position = transform.position;
                tempArrow.transform.SetParent(dynamicObjects);
                ArrowPool.Remove(tempArrow);

                tempArrow.transform.LookAt(target.position);
                tempArrow.GetComponent<Rigidbody>().AddForce(tempArrow.transform.forward * 1000);
                StartCoroutine(ReturnArrows(tempArrow));
            }


            #region 
            enemyCheck = false;


            Collider[] hits = Physics.OverlapSphere(transform.position, range);
            foreach (Collider hitted in hits) {
                if (hitted.transform.CompareTag("Enemy")) {
                    enemyCheck = true;
                    break;
                }
            }

            if (!enemyCheck) target = null;

            #endregion



            tempoPassado += Time.deltaTime;
        }
    }

    IEnumerator ReturnArrows(GameObject arrow) {
        yield return new WaitForSeconds(3);
        arrow.transform.SetParent(GameObject.Find("ArrowPool").transform);
        arrow.gameObject.SetActive(false);
        ArrowPool.Add(arrow);

    }

    #region ULTRA-IMPORTANT J2 - PARA O CALIFE
    /*
                                     Z             
                               Z                   
                .,.,        z           
              (((((())    z             
             ((('_  _`) '               
             ((G   \ |)                 
            (((`   " ,                  
             .((\.:~:          .--------------.    
             __.| `"'.__      | \              |     
            .~~   `---'   ~.    |  .             :     
            /                `   |   `-.__________)     
            |             ~       |  :             :   
            |                     |  :  |              
            |    _                |     |   [ ##   :   
            \    ~~-.            |  ,   oo_______.'   
            `_   ( \) _____/~~~~ `--___              
            | ~`-)  ) `-.   `---   ( - a:f -         
            |   '///`  | `-.                         
            |     | |  |    `-.                      
            |     | |  |       `-.                   
            |     | |\ |                             
            |     | | \|                             
            `-.  | |  |                             
              `-| '
 */
    #endregion
}
