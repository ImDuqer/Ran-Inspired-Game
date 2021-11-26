using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MenuController : MonoBehaviour {
    [SerializeField] ScrollButtonController ContinueButton;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Animator settingsPanel;
    [SerializeField] Image MenuImage;
    [SerializeField] Sprite AlterMenu;
    public static bool desicionMade = false;

    [SerializeField] AudioMixer AudioMixer;
    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown ResolutionDropdown;


    private void Awake() {
        ResourceTracker.WEEK = 1;
        ResourceTracker.WAVE = 1;
        if (ResourceTracker.WEEK == 1 && ResourceTracker.WAVE == 1) {
            if(ContinueButton != null) ContinueButton.blocked = true;
        }
        if (MenuImage != null && PlayerPrefs.GetInt("HighWeek") >= 9) MenuImage.sprite = AlterMenu;
    }

    private void Start() {

        #region settings

        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                CurrentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
        #endregion
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) CreditsBack();
    }
    public void SetResolution(int ResolutionIndex) {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume) {
        AudioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
    public void NewGame() {
        StartCoroutine(LoadSceneMethod(1));
        PlayerPrefs.SetInt("CurrentWeek", 0);
        ResourceTracker.WEEK = 1;
        ResourceTracker.WAVE = 1;
        desicionMade = true;
    }
    public void Continue() {
        if (!desicionMade) StartCoroutine(LoadSceneMethod("Gameplay"));
        desicionMade = true;
    }
    public void Settings() {
        settingsPanel.SetTrigger("popin");
        desicionMade = true;
    }
    public void SettingsBack() {
        settingsPanel.SetTrigger("popout");
        desicionMade = false;
    }
    public void Credits() {
        if (!desicionMade) StartCoroutine(LoadSceneMethod("credits"));
        desicionMade = true;
    }
    public void CreditsBack() {
        StartCoroutine(LoadSceneMethod(0));
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
