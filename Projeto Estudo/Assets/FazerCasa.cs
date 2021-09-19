using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FazerCasa : MonoBehaviour
{
    public int moedas = 10;
    public GameObject Casa;

    void Start()
    {
        Casa.SetActive(false);
    }

    void Update()
    {
        Construir();
    }

    public void Construir()
    {
        if(moedas >= 10)
        {
            Casa.SetActive(true);
        }
        else
        {
            Casa.SetActive(false);
        }
    }
}
