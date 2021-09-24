using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TesteAdicionarMoedas : MonoBehaviour
{
    public int moedas = 10;
    

    public void MaisMoedas()
    {
        moedas += 10;
    }
}