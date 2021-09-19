using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstruirNova : MonoBehaviour
{
    public GameObject Casaa;
    public GameObject BotaoCasa;
    public int moeda;
    public int populacao;
    public int tempoComeca = 0;

    void Start()
    {
        Casaa.SetActive(false);
        populacao = 0;
        moeda = 10;
    }

    public void Fazer()
    {
        if(moeda >= 10)
        {
            //Casaa.SetActive(true);
            tempoComeca = 1;
            StartCoroutine(Coisa());
        }   
    }

    IEnumerator Coisa()
    {
        if(tempoComeca == 1)
        {
            yield return new WaitForSeconds(1f);
            Casaa.SetActive(true);
            populacao += 10;
            moeda -= 10;
            BotaoCasa.SetActive(false);
        }
    }
}
