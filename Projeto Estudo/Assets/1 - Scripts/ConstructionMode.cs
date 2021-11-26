using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
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


    [SerializeField] AudioMixer AudioMixer;
    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown ResolutionDropdown;

    void Awake() {
        ArcherStands = GameObject.FindGameObjectsWithTag("Archer");
        ConstructionStands = GameObject.FindGameObjectsWithTag("Construction");
        TowerStands = GameObject.FindGameObjectsWithTag("Tower");
        FighterStands = GameObject.FindGameObjectsWithTag("Fighter");
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
            if (Input.GetKeyDown(KeyCode.Q)) FighterButton();
            if (Input.GetKeyDown(KeyCode.W)) ArcherButton();
            if (Input.GetKeyDown(KeyCode.E)) ConstructionButton();
            if (Input.GetKeyDown(KeyCode.R)) TowerButton();

        }
        else {
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
        Time.timeScale = Time.timeScale == 1 ? 2.5f : 1f;
    }
    public void TogglePause() {
        paused.SetActive(!paused.activeSelf);
        Time.timeScale = Time.timeScale > 0 ? 0 : 1f; ;
    }

    void TurnObj(GameObject[] array, bool state){
        foreach (GameObject obj in array) {
            obj.SetActive(state);
        }
    }


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
            onConstruction = true;
            return;
        }
        if (onConstruction) {
            onConstruction = false;
            TurnObj(ArcherStands, false);
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

}