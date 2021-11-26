using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitByobu : MonoBehaviour {
    [SerializeField] bool menu = false;
    void OnEnable() {
        if(!menu) SceneManager.LoadScene("gameplay");
        else SceneManager.LoadScene("creditos");
    }
}
