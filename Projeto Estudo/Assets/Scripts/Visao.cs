using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visao : MonoBehaviour
{
    float sensibilidade = 300f;
    float rotacaoX = 0f;
    float distanciaVisao = 20f;
    public Transform alvoRaycast;
    public Transform corpo;
    public LayerMask alvosVisao;
    public GameObject textoItem;
    public bool testeItem;

    void Start()
    {
        testeItem = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanciaVisao);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidade * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidade * Time.deltaTime;

        rotacaoX -= mouseY;
        rotacaoX = Mathf.Clamp(rotacaoX, -90f, 90f);
        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
        corpo.Rotate(Vector3.up * mouseX);

        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(raio.origin, raio.direction * 20, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(raio, out hit, 5, alvosVisao))
        {
            if(hit.transform == alvoRaycast)
            {
                textoItem.SetActive(true);
                testeItem = true;
            }
        }

        else
        {
            textoItem.SetActive(false);
            testeItem = false;
        }
    }
}
