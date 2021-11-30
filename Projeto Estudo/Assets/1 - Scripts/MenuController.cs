using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using FMODUnity;

public class MenuController : MonoBehaviour {
    [SerializeField] ScrollButtonController ContinueButton;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Animator settingsPanel;
    [SerializeField] Image MenuImage;
    [SerializeField] Sprite AlterMenu;
    public static bool desicionMade = false;


    

    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown ResolutionDropdown;
    [SerializeField] Toggle fullscreen;

    [SerializeField] TextMeshProUGUI high;


    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    //float volume;
    string uIPath = "vca:/UI";
    string sFXPath = "vca:/SFX";
    string musicPath = "vca:/Music";
    FMOD.Studio.VCA uIVCA;
    FMOD.Studio.VCA sFXVCA;
    FMOD.Studio.VCA musicVCA;


    private void Awake() {
        Time.timeScale = 1;
        if (SaveSystem.LoadState() == null) {
            if(ContinueButton != null) ContinueButton.blocked = true;
        }
        if (MenuImage != null && PlayerPrefs.GetInt("HighWeek") >= 9) MenuImage.sprite = AlterMenu;
    }

    private void Start() {

        #region audio

        uIVCA = FMODUnity.RuntimeManager.GetVCA(uIPath);
        sFXVCA = FMODUnity.RuntimeManager.GetVCA(sFXPath);
        musicVCA = FMODUnity.RuntimeManager.GetVCA(musicPath);

        if(!PlayerPrefs.HasKey("SFXVolumePreference")) PlayerPrefs.SetFloat("SFXVolumePreference" , 1);
        if (!PlayerPrefs.HasKey("MusicVolumePreference")) PlayerPrefs.SetFloat("MusicVolumePreference", 1);

        if (sFXSlider != null) sFXSlider.value = PlayerPrefs.GetFloat("SFXVolumePreference");
        if (musicSlider != null) musicSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");

        if (musicSlider != null) musicVCA.setVolume(musicSlider.value);
        if (sFXSlider != null) sFXVCA.setVolume(sFXSlider.value);
        if (sFXSlider != null) uIVCA.setVolume(sFXSlider.value);

        #endregion

        #region settings

        int CurrentResolutionIndex = 0;
        resolutions = Screen.resolutions;

        if(ResolutionDropdown != null) ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++) {
            string Option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(Option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                CurrentResolutionIndex = i;
            }
        }

        if (ResolutionDropdown != null) ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();



        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
        if(fullscreen != null) fullscreen.isOn = Screen.fullScreen;


        Resolution resolution = resolutions[PlayerPrefs.GetInt("ResolutionPreference")];

        if (ResolutionDropdown != null) ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        if (ResolutionDropdown != null) ResolutionDropdown.RefreshShownValue();
        if (ResolutionDropdown != null) Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        #endregion



        //Debug.Log("WEEK: " + PlayerPrefs.GetInt("CurrentWeek"));


        if (high != null) high.text = "Semana mais alta:  " + PlayerPrefs.GetInt("HighWeek");


    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) CreditsBack();
    }
    public void SetResolution(int ResolutionIndex) {
        PlayerPrefs.SetInt("ResolutionPreference", ResolutionIndex);
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSFXVolume(float volume) {
        PlayerPrefs.SetFloat("SFXVolumePreference", volume);
        uIVCA.setVolume(volume);
        sFXVCA.setVolume(volume);
    }
    public void SetMusicVolume(float volume) {
        PlayerPrefs.SetFloat("MusicVolumePreference", volume);
        musicVCA.setVolume(volume);
    }

    public void SetFullscreen(bool isFullscreen) {
        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
        Screen.fullScreen = isFullscreen;
    }
    public void NewGame() {
        StartCoroutine(LoadSceneMethod(1));
        PlayerPrefs.SetInt("CurrentWeek", 0);
        ResourceTracker.WEEK = 1;
        ResourceTracker.WAVE = 1;
        SaveSystem.ClearState();
        desicionMade = true;
    }
    public void Continue() {
        if (!desicionMade) StartCoroutine(LoadSceneMethod("gameplay"));
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
