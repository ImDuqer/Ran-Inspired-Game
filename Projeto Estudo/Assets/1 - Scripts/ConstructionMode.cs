using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConstructionMode : MonoBehaviour {


    [SerializeField] Animator settingsPanel;
    [SerializeField] GameObject paused;
    GameObject[] ArcherStands;
    GameObject[] TowerStands;
    GameObject[] ConstructionStands;
    GameObject[] FighterStands;
    bool onConstruction = false;
    bool CC = false;
    bool AC = false;
    bool TC = false;
    bool FC = false;

    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown ResolutionDropdown;
    [SerializeField] Toggle fullscreen;


    List<bool> boughtUnits = new List<bool>();


    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    float volume;
    string uIPath = "vca:/UI";
    string sFXPath = "vca:/SFX";
    string musicPath = "vca:/Music";
    FMOD.Studio.VCA uIVCA;
    FMOD.Studio.VCA sFXVCA;
    FMOD.Studio.VCA musicVCA;
    bool saveTime = true;


    void UpdateBought() {
        boughtUnits.Clear();
        foreach (GameObject stand in ArcherStands) {
            boughtUnits.Add(stand.GetComponent<ArcherStand>().bought);
        }
        foreach (GameObject stand in TowerStands) {
            //Debug.Log(TowerStands);
            //Debug.Log(stand);
            //Debug.Log(stand.GetComponent<TowerStand>());
            //Debug.Log(stand.GetComponent<TowerStand>().bought);
            boughtUnits.Add(stand.GetComponent<TowerStand>().bought);
        }
        foreach (GameObject stand in ConstructionStands) {
            boughtUnits.Add(stand.GetComponent<ConstructionStand>().bought);
        }
        foreach (GameObject stand in FighterStands) {
            boughtUnits.Add(stand.GetComponent<SamuraiStand>().bought);
        }
    }


    public bool[] GetUnitsForSave() {
        UpdateBought();
        bool[] returning = new bool[boughtUnits.Count];

        for(int i = 0; i < boughtUnits.Count; i++) {
            returning[i] = boughtUnits[i];
        }

        return returning;
    }

    void SetUnits(bool[] loadedArray) {
        UpdateBought();

        int n = 0;
        foreach (GameObject stand in ArcherStands) {
            stand.GetComponent<ArcherStand>().bought = loadedArray[n];
            if(loadedArray[n]) stand.SetActive(true);
            n++;
        }
        foreach (GameObject stand in TowerStands) {
            stand.GetComponent<TowerStand>().bought = loadedArray[n];
            if (loadedArray[n]) stand.SetActive(true);
            n++;
        }
        foreach (GameObject stand in ConstructionStands) {
            stand.GetComponent<ConstructionStand>().bought = loadedArray[n];
            if (loadedArray[n]) stand.SetActive(true);
            n++;
        }
        foreach (GameObject stand in FighterStands) {
            stand.GetComponent<SamuraiStand>().bought = loadedArray[n];
            if (loadedArray[n]) stand.SetActive(true);
            n++;
        }



        for (int i = 0; i < loadedArray.Length; i++) {
            boughtUnits[i] = loadedArray[i];
        }
    }



    public void SaveGame() {
        Slider slider = GameObject.Find("VidadoCastelo").GetComponent<Slider>();
        CardsButton cb = GameObject.Find("CardsObject").GetComponent<CardsButton>();
        SaveSystem.SaveState((int)slider.value, ResourceTracker.POINTS, ResourceTracker.CURRENT_POPULATION, ResourceTracker.MAX_POPULATION, GetUnitsForSave(), EnemySpawner.currentWeek, cb.cardsAmmount);
        //int health, int points, int pop, int maxPop, bool[] unitsList, int week, int[] cards
    }

    void LoadGame() {
        PlayerData data = SaveSystem.LoadState();
        GameObject.Find("VidadoCastelo").GetComponent<Slider>().value = data.health;
        ResourceTracker.POINTS = data.points;
        ResourceTracker.CURRENT_POPULATION = data.pop;
        ResourceTracker.MAX_POPULATION = data.maxPop;
        SetUnits(data.unitsList);
        EnemySpawner.currentWeek = data.week;
        PlayerPrefs.SetInt("CurrentWeek", data.week);
        ResourceTracker.WEEK = data.week;
        CardsButton cb = GameObject.Find("CardsObject").GetComponent<CardsButton>();
        cb.cardsAmmount = data.cards;
        cb.UpdateCards();
    }

    void Awake() {




        ArcherStands = GameObject.FindGameObjectsWithTag("ArcherStand");
        ConstructionStands = GameObject.FindGameObjectsWithTag("Construction");
        TowerStands = GameObject.FindGameObjectsWithTag("Tower");
        FighterStands = GameObject.FindGameObjectsWithTag("FighterStand");








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
        Screen.fullScreen = isFullscreen;
    }
    private void Start() {
        if (SaveSystem.LoadState() != null) LoadGame();
        #region audio

        uIVCA = FMODUnity.RuntimeManager.GetVCA(uIPath);
        sFXVCA = FMODUnity.RuntimeManager.GetVCA(sFXPath);
        musicVCA = FMODUnity.RuntimeManager.GetVCA(musicPath);


        sFXSlider.value = PlayerPrefs.GetFloat("SFXVolumePreference");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");

        musicVCA.setVolume(musicSlider.value);
        sFXVCA.setVolume(sFXSlider.value);
        uIVCA.setVolume(sFXSlider.value);

        #endregion


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
        //ResolutionDropdown.value = CurrentResolutionIndex;
        //ResolutionDropdown.RefreshShownValue();



        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
        fullscreen.isOn = Screen.fullScreen;


        Resolution resolution = resolutions[PlayerPrefs.GetInt("ResolutionPreference")];

        ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        ResolutionDropdown.RefreshShownValue();
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        #endregion
    }
    public void Settings() {
        settingsPanel.SetTrigger("popin");
    }
    public void SettingsBack() {
        settingsPanel.SetTrigger("popout");
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && EnemySpawner.currentGamePhase == GamePhase.ACTION_PHASE) {
            ToggleFastForward();
        }
        if (Input.GetKeyDown(KeyCode.D) && EnemySpawner.currentGamePhase == GamePhase.ACTION_PHASE) {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(!settingsPanel.gameObject.transform.GetChild(0).gameObject.activeSelf) Settings();

            else SettingsBack();
        }
        if (EnemySpawner.currentGamePhase == GamePhase.SETUP_PHASE) {

            if (saveTime) {
                SaveGame();
                saveTime = false;
            }


            if (Input.GetKeyDown(KeyCode.Q)) FighterButton();
            if (Input.GetKeyDown(KeyCode.W)) ArcherButton();
            if (Input.GetKeyDown(KeyCode.E)) ConstructionButton();
            if (Input.GetKeyDown(KeyCode.R)) TowerButton();

        }
        else {

            saveTime = true;
            AC = false;
            CC = false;
            TC = false;
            FC = false;
            onConstruction = false;
            TurnObj(ArcherStands, false);
            TurnObj(ConstructionStands, false);
            TurnObj(TowerStands, false);
            TurnObj(FighterStands, false);
        }
    }

    public void ToggleFastForward() {
        if (EnemySpawner.currentGamePhase == GamePhase.ACTION_PHASE) Time.timeScale = Time.timeScale == 1 ? 2.5f : 1f;
    }
    public void TogglePause() {
        if (EnemySpawner.currentGamePhase == GamePhase.ACTION_PHASE) {
            paused.SetActive(!paused.activeSelf);
            Time.timeScale = Time.timeScale > 0 ? 0 : 1f;
        }
    }
    //vsf gustavo
    void TurnObj(GameObject[] array, bool state){
        foreach (GameObject obj in array) {
            obj.SetActive(state);
        }
    }
    /*
    void TurnActivate(GameObject[] array, bool state, int x) {
        foreach (GameObject obj in array) {
            switch (x) {
                case 0:
                    obj.GetComponent<ArcherStand>().activate = true;
                    break;
            }
        }
    }*/

    public void ArcherButton() {
        if (CC || TC || FC) {
            TurnObj(ConstructionStands, false);
            TurnObj(TowerStands, false);
            TurnObj(FighterStands, false);
            AC = true;
            CC = false;
            TC = false;
            FC = false;
            TurnObj(ArcherStands, true);
            //TurnActivate(ArcherStands, true, 0);
            onConstruction = true;
            return;
        }
        if (onConstruction) {
            onConstruction = false;
            TurnObj(ArcherStands, false);
            //TurnActivate(ArcherStands, false, 0);
            return;
        }
        AC = true;
        TurnObj(ArcherStands, true);
        onConstruction = true;
    }
    public void ConstructionButton() {
        if (AC || TC || FC) {
            TurnObj(ArcherStands, false);
            TurnObj(TowerStands, false);
            TurnObj(FighterStands, false);
            AC = false;
            TC = false;
            FC = false;
            CC = true;
            TurnObj(ConstructionStands, true);
            onConstruction = true;
            return;
        }
        if (onConstruction) {
            onConstruction = false;
            TurnObj(ConstructionStands, false);
            return;
        }
        CC = true;
        TurnObj(ConstructionStands, true);
        onConstruction = true;
    }
    public void TowerButton() {
        if (AC || CC || FC) {
            TurnObj(ConstructionStands, false);
            TurnObj(ArcherStands, false);
            TurnObj(FighterStands, false);
            TC = true;
            AC = false;
            CC = false;
            FC = false;
            TurnObj(TowerStands, true);
            onConstruction = true;
            return;
        }
        if (onConstruction) {
            onConstruction = false;
            TurnObj(TowerStands, false);
            return;
        }
        TC = true;
        TurnObj(TowerStands, true);
        onConstruction = true;
    }
    public void FighterButton() {
        if (AC || CC || TC) {
            TurnObj(ConstructionStands, false);
            TurnObj(ArcherStands, false);
            TurnObj(TowerStands, false);
            TC = false;
            AC = false;
            CC = false;
            FC = true;
            TurnObj(FighterStands, true);
            onConstruction = true;
            return;
        }
        if (onConstruction) {
            onConstruction = false;
            TurnObj(FighterStands, false);
            return;
        }
        FC = true;
        TurnObj(FighterStands, true);
        onConstruction = true;
    }

    public void Menu(){
        SceneManager.LoadScene(0);
    }

}