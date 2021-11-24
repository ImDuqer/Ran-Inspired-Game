using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour {

    public GameObject gameCanvas;
    public GameObject[] characters;
    public TextAsset[] inkFiles;
    public GameObject textBox;
    public GameObject[] choiceButtons;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool isTalking = false;
    bool typing = false;

    static Story story;
    TextMeshProUGUI nametag;
    string _nameTag = " ";
    TextMeshProUGUI message;
    string spriteTags;
    string charTags;
    static Choice choiceSelected;
    bool skip = false;
    Coroutine writingCoroutine;
    string currentSentence;
    bool waitingForContinue = false;

    [SerializeField] TextMeshProUGUI myWC;

    bool[] Shown = new bool[9];


    void OnEnable() {
        //Debug.Log(EnemySpawner.currentWeek);
        for (int i = 0; i < Shown.Length; i++) {
            Shown[i] = false;
        }
        story = new Story(inkFiles[EnemySpawner.currentWeek].text);
        nametag = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        choiceSelected = null;
        //ParseTags();
        DoDialog();
    }

    void Update() {
        if (waitingForContinue) {

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                Debug.Log("Aaa-");
                choiceSelected = story.currentChoices[0];
                story.ChooseChoiceIndex(0);
                waitingForContinue = false;
            }
        }
        if (typing && Input.GetMouseButtonDown(0) && writingCoroutine != null) {

            Debug.Log("Bbb-");
            typing = false;
            StopCoroutine(writingCoroutine);
            writingCoroutine = null;
            message.text = currentSentence;
        }
        else if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !typing) {
            Debug.Log("Fff-");
            Debug.Log("Can Continue? " + story.canContinue); 
            Debug.Log("Current Choices: " + story.currentChoices.Count);
            DoDialog();
        }
        //Debug.Log("waiting? " + waitingForContinue);


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            EnemySpawner.currentWeek = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            EnemySpawner.currentWeek = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            EnemySpawner.currentWeek = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            EnemySpawner.currentWeek = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            EnemySpawner.currentWeek = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            EnemySpawner.currentWeek = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            EnemySpawner.currentWeek = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            EnemySpawner.currentWeek = 8;
        }

        //Debug.Log("current text: " + story.currentText);
    }



    void DoDialog() {
        //Is there more to the story?
        if (story.canContinue) {
            AdvanceDialogue();

            //Are there any choices?
            if (story.currentChoices.Count != 0) {
                StartCoroutine(ShowChoices());
            }
        }
        else if (story.currentChoices.Count <= 0) {
            FinishDialogue();
        }

    }

    void FinishDialogue() {
        for (int i = 0; i < Shown.Length; i++) {
            Shown[i] = false;
        }
        foreach(GameObject character in characters) {
            character.SetActive(false);
        }
        optionPanel.SetActive(false);
        gameCanvas.SetActive(true);
        gameObject.SetActive(false);
        CameraPanning.shouldPanCamera = true;
        myWC.text = "Week " + (EnemySpawner.currentWeek+1);
        //myWC.transform.parent.GetComponent<Animator>().SetTrigger("showup");

    }

    
    void AdvanceDialogue() {
        choiceSelected = null;
        Debug.Log("Can story continue? " + story.canContinue);
        if (story.canContinue) {
            currentSentence = story.Continue();
            Debug.Log("Current Sentence Held (Advance Dialogue): " + currentSentence);
            if (story.currentTags.Count != 0) ParseTags();
            StopAllCoroutines();
            writingCoroutine = StartCoroutine(TypeSentence(currentSentence));
            if (currentSentence == "" && EnemySpawner.currentWeek == 6) FinishDialogue();
        }
        else if (story.currentChoices.Count <= 0) { 
            FinishDialogue();
        }
    }



    void SkipAnwser() {
        Debug.Log("Can story continue? " + story.canContinue);
        if (story.canContinue) {
            currentSentence = story.Continue();
            Debug.Log("Current Sentence Held (Skip Dialogue): " + currentSentence);
        }
        AdvanceDialogue();
        if (story.currentChoices.Count != 0) {
            StartCoroutine(ShowChoices());
        }
    }


    IEnumerator TypeSentence(string sentence) {
        
        typing = true;
        message.text = "";
        yield return null;

        foreach (char letter in sentence.ToCharArray()) {
            //.Log("Aaa-");
            message.text += letter;
            yield return new WaitForSeconds(0.08f);
        }


        typing = false;
        yield return null;
    }


    IEnumerator ShowChoices() {
        //Debug.Log("story.currentChoices.Count == 2? " + story.currentChoices.Count);

        choiceSelected = null;
        while (typing) {
            yield return new WaitForEndOfFrame();
        }

        if (story.currentChoices.Count == 2) {

            //Debug.Log("Should have showed options");

            Debug.Log("CCC-");
            List<Choice> _choices = story.currentChoices;

            optionPanel.SetActive(true);
            for (int i = 0; i < _choices.Count; i++) {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
                choiceButtons[i].GetComponent<Selectable>().element = _choices[i];
                //choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => { choiceButtons[i].GetComponent<Selectable>().Decide(); });
            }

        }
        else {
            //Debug.Log("Should have updated bool");

            Debug.Log("Ddd-");
            waitingForContinue = true;
        }
        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }




    public static void SetDecision(object element) {
        
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }


    void AdvanceFromDecision() {
        if (optionPanel.activeSelf) {

            for (int i = 0; i < optionPanel.transform.childCount; i++) {
                choiceButtons[i].SetActive(false);
            }
            optionPanel.SetActive(false);
            choiceSelected = null;
            //SkipAnwser();
            if (EnemySpawner.currentWeek != 6) SkipAnwser();
            else {
                waitingForContinue = true;
                AdvanceDialogue();
            }
        }
        else {
            choiceSelected = null;

            AdvanceDialogue();
        }
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags() {
        charTags = story.currentTags[0];
        spriteTags = story.currentTags[1];

        string spriteParam = spriteTags.Split(' ')[1];
        string charParam = charTags.Split(' ')[1];


        SetChar(charParam); 
        SetSprite(spriteParam);

    }

    void SetChar(string _char) {
        switch (_char) {
            case "Sato":
                _nameTag = "Minsei Sato";
                break;

            case "conselheiro":
                _nameTag = "Conselheiro";
                break;

            case "Takashi":
                _nameTag = "Minsei Takashi";
                break;

            case "soldado":
                _nameTag = "Soldado Minsei";
                break;

            case "soldado1":
                _nameTag = "Soldado Minsei 1";
                break;

            case "soldado2":
                _nameTag = "Soldado Minsei 2";
                break;

            case "soldado3":
                _nameTag = "Soldado Minsei 3";
                break;

            case "Kaede":
                _nameTag = "Kaede";
                break;

            case "nada":
                _nameTag = " ";
                break;

            case "Hidetora":
                _nameTag = "Ichimonji Hidetora";
                break;

            case "soldadoInimigo":
                _nameTag = "Soldado do clã Ichimonji";
                break;

            case "conselheiroHidetora":
                _nameTag = "Conselheiro de Hidetora";
                break;

            default:
                _nameTag = "";
                break;
        }

        nametag.text = _nameTag;
    }

    void SetSprite(string _sprite) {
        switch (_sprite) {
            case "Sato":
                UpdateAnimation(characters[0]);
                Shown[0] = true;
                break;

            case "conselheiro":
                UpdateAnimation(characters[1]);
                Shown[1] = true;
                break;

            case "Takashi":
                Animator animator = characters[2].GetComponent<Animator>();
                if (Shown[2]) {
                    animator.SetTrigger("TurnOn");
                }
                else {
                    characters[2].SetActive(true);
                    Shown[2] = true;
                }
                int i = -1;
                foreach (bool b in Shown) {
                    i++;
                    if (b && i != 2 && (CheckAnim("TurnOnSprite", characters[i].GetComponent<Animator>()) || CheckAnim("FirstPopUp1", characters[i].GetComponent<Animator>()))) {
                        Animator _animator = characters[i].GetComponent<Animator>();
                        _animator.SetTrigger("TurnOff");
                    }
                }
                break;

            case "soldado":
                UpdateAnimation(characters[3]);
                Shown[3] = true;
                break;

            case "Kaede":
                UpdateAnimation(characters[4]);
                Shown[4] = true;
                break;

            case "nada":
                UpdateAnimation(characters[5]);
                Shown[5] = true;
                break;

            case "Hidetora":
                UpdateAnimation(characters[6]);
                Shown[6] = true;
                break;

            case "soldadoInimigo":
                UpdateAnimation(characters[7]);
                Shown[7] = true;
                break;

            case "conselheiroHidetora":
                UpdateAnimation(characters[8]);
                Shown[8] = true;
                break;

            default:

                break;
        }
    }

    void UpdateAnimation(GameObject character) {
        Animator animator = character.GetComponent<Animator>();
        if (!character.activeSelf) {
            character.SetActive(true);
        }
        else {
            animator.SetTrigger("TurnOn");
        }
        int i = -1;
        foreach (bool b in Shown) {
            i++;
            if (b && characters[i] != character && i != 2 && !CheckAnim("TurnFullOffSprite", characters[i].GetComponent<Animator>())) {
                Animator _animator = characters[i].GetComponent<Animator>();
                _animator.SetTrigger("TurnFullOff");
            }
            if(i == 2 && characters[2].GetComponent<Animator>().isActiveAndEnabled && !CheckAnim("TurnOffSprite", characters[i].GetComponent<Animator>())) {
                characters[2].GetComponent<Animator>().SetTrigger("TurnOff");
            }
        }
    }

    bool CheckAnim(string name, Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    #region
    void SetAnimation(string _name) {
        SpriteControllerScript cs = GameObject.FindObjectOfType<SpriteControllerScript>();
        cs.PlayAnimation(_name);
    }
    void SetTextColor(string _color) {
        switch (_color) {
            case "red":
                message.color = Color.red;
                break;
            case "blue":
                message.color = Color.cyan;
                break;
            case "green":
                message.color = Color.green;
                break;
            case "white":
                message.color = Color.white;
                break;
            default:
                Debug.Log($"{_color} is not available as a text color");
                break;
        }
    }
    #endregion
}