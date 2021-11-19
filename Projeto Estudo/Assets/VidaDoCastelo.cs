using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VidaDoCastelo : MonoBehaviour
{
    public static int VIDA;

    Slider myS;

    void Start()
    {
        myS = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        VIDA = (int)myS.value;
        if(VIDA <= 0) {
            SceneManager.LoadScene(0);
        }
    }
}
