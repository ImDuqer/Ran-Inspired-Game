using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Derrota : MonoBehaviour {



    float timer;

    void OnEnable() {

        Time.timeScale = 0;
    }

    private void Update() {
        timer += Time.unscaledDeltaTime;
        if (timer >= 15) BackToMenu();
        if(Input.anyKeyDown || Input.GetMouseButtonDown(0)) BackToMenu();
    }

    public void Byobu2() {
        SceneManager.LoadScene("Byobu2");
    }

    void BackToMenu() {

        SceneManager.LoadScene(0);
    }
}
