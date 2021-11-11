using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    [SerializeField] ScrollButtonController ContinueButton;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject settingsPanel;
    public static bool desicionMade = false;
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
        if (!desicionMade) StartCoroutine(LoadSceneMethod(1));
        desicionMade = true;
    }
    public void Settings() {
        settingsPanel.SetActive(true);
        desicionMade = true;
    }
    public void SettingsBack() {
        settingsPanel.SetActive(false);
        desicionMade = true;
    }
    public void Credits() {
        if (!desicionMade) StartCoroutine(LoadSceneMethod("credits"));
        desicionMade = true;
    }
    public void Quit() {
        if (!desicionMade) Application.Quit();
        desicionMade = true;
    }
    IEnumerator LoadSceneMethod(string sceneString) {
        yield return new WaitForSeconds(0.7f);
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneString);
    }
    IEnumerator LoadSceneMethod(int sceneIndex) {
        yield return new WaitForSeconds(0.7f);
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneIndex);
    }
}
