using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    [SerializeField] ScrollButtonController ContinueButton;
    bool desicionMade = false;
    private void Awake() {
        ResourceTracker.WEEK = 1;
        ResourceTracker.WAVE = 1;
        if (ResourceTracker.WEEK == 1 && ResourceTracker.WAVE == 1) {
            ContinueButton.blocked = true;
        }
        
    }

    public void NewGame() {
        StartCoroutine(LoadSceneMethod(1));
        ResourceTracker.WEEK = 1;
        ResourceTracker.WAVE = 1;
        desicionMade = true;
    }
    public void Continue() {
        StartCoroutine(LoadSceneMethod(1));
        desicionMade = true;
    }
    public void Settings() {
        desicionMade = true;
    }
    public void Credits() {
        StartCoroutine(LoadSceneMethod("credits"));
        desicionMade = true;
    }
    public void Quit() {
        Application.Quit();
        desicionMade = true;
    }
    IEnumerator LoadSceneMethod(string sceneString) {
        if (!desicionMade) {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(sceneString);
        }
    }
    IEnumerator LoadSceneMethod(int sceneIndex) {
        if (!desicionMade) {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
