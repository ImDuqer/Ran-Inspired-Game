using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaMouse : MonoBehaviour
{
    public GameObject Moradia;
    public GameObject MenuConstrucao;
    //public int moeda = 10;

    private void Start()
    {
        MenuConstrucao.SetActive(false);
    }
    void Update()
    {
        //if(moeda == 10)
        //{
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(raio, out hit))
                {
                    if(hit.transform.tag == "Casa")
                    {
                        MenuConstrucao.SetActive(true);
                        //Vector3 posicao = hit.point + Vector3.up;
                        //Instantiate(Moradia, posicao, Quaternion.identity);
                        //moeda -= 10;
                    }
                }
            }
        //}   
    }
}
