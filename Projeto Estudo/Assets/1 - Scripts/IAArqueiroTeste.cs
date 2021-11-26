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
    float originalCD;
    float arcoCDBuff;
    float tempoPassado;
    [SerializeField] bool gizmos;
    [SerializeField] bool debugTarget;
    [SerializeField] Material[] myMaterials;
    MeshRenderer myMR;
    public static List<GameObject> ArrowPool = new List<GameObject>();
    public static bool filledPool;
    GameObject shotStart;
    int HP = 2;
    Animator myAnim;
    bool enemyCheck;

    [HideInInspector] public bool inCombat;

    Transform dynamicObjects;

    public GameObject originalParent;

    void OnEnable() {
        myAnim = GetComponent<Animator>();
        arcoCDBuff = arcoCD * 0.8f;
        originalCD = arcoCD;
        shotStart = transform.GetChild(transform.childCount-1).gameObject;
        dynamicObjects = GameObject.Find("Dynamic Objects").transform;
        if (!filledPool) {
            filledPool = true;
            foreach (Transform arrow in GameObject.Find("ArrowPool").transform) {
                ArrowPool.Add(arrow.gameObject);
            }
        }
        myMR = transform.GetChild(6).GetComponent<MeshRenderer>();
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
            //myMR.material = myMaterials[0];
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
            //myMR.material = myMaterials[1];




            arcoCD = CardsButton.ATTACKSPEEDBUFF ? arcoCDBuff : originalCD;

            if (tempoPassado >= arcoCD && target != null) {
                tempoPassado = 0;
                myAnim.SetTrigger("Shoot");
                GameObject tempArrow = ArrowPool[0];
                tempArrow.SetActive(true);
                tempArrow.transform.position = shotStart.transform.position;
                tempArrow.transform.SetParent(dynamicObjects);

                ArrowPool.Remove(tempArrow);
                tempArrow.GetComponent<Arrow>().SetTarget(target.gameObject, this);
                //tempArrow.transform.LookAt(target.position);
                //tempArrow.GetComponent<Rigidbody>().AddForce(tempArrow.transform.forward * 1000);
                target = null;
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
        ReturnArrow(arrow);

    }

    public void TakeDamage() {
        if (CardsButton.DAMAGEDEBUFF) {
            HP -= 1;
        }
        else HP -= 2;

        if (HP <= 0) EnemyReset();
    }


    public void EnemyReset() {
        originalParent.GetComponent<ArcherStand>().bought = false;
        transform.SetParent(originalParent.transform);
        gameObject.SetActive(false);
        HP = 2;
    }


    public void ReturnArrow(GameObject arrow) {
        arrow.transform.SetParent(GameObject.Find("ArrowPool").transform);
        arrow.SetActive(false);
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
