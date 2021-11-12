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
    List<string> tags;
    static Choice choiceSelected;
    bool skip = false;
    Coroutine writingCoroutine;
    string currentSentence;

    [SerializeField] TextMeshProUGUI myWC;
    void Start() {
        
    }

    void OnEnable() {
        story = new Story(inkFiles[EnemySpawner.currentWeek].text);
        nametag = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tags = new List<string>();
        choiceSelected = null;
        DoDialog();
    }

    private void Update() {
        if (typing && Input.GetMouseButtonDown(0) && writingCoroutine != null) {
            typing = false;
            StopCoroutine(writingCoroutine);
            writingCoroutine = null;
            message.text = currentSentence;
            typing = false;
        }
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !typing) {
            DoDialog();
        }

    }

    void DoDialog() {
        //Is there more to the story?
        if (story.canContinue) {
            nametag.text = _nameTag;
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

    private void FinishDialogue() {
        gameCanvas.SetActive(true);
        gameObject.SetActive(false);
        CameraPanning.shouldPanCamera = true;
        myWC.text = "Week " + EnemySpawner.currentWeek;
        myWC.GetComponent<Animator>().SetTrigger("showup");
    }

    
    void AdvanceDialogue() {
        currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        writingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }


    IEnumerator TypeSentence(string sentence) {
        typing = true;
        message.text = "";
        yield return null;

        foreach (char letter in sentence.ToCharArray()) {
            message.text += letter;
            yield return new WaitForSeconds(0.08f);
        }

        //SpriteControllerScript tempSpeaker = GameObject.FindObjectOfType<SpriteControllerScript>();
        //if (tempSpeaker.isTalking) {
        //    SetAnimation("idle");
        //}
        typing = false;
        yield return null;
    }


    IEnumerator ShowChoices() {
        Debug.Log("There are choices need to be made here!");

        while (typing) {
            yield return null;
        }
        if (story.currentChoices.Count == 2) {
            List<Choice> _choices = story.currentChoices;

            for (int i = 0; i < _choices.Count; i++) {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
                choiceButtons[i].GetComponent<Selectable>().element = _choices[i];
                choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => { choiceButtons[i].GetComponent<Selectable>().Decide(); });
            }

            optionPanel.SetActive(true);
        }
        else {
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                story.ChooseChoiceIndex(0);
            }
        }
        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(object element) {
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision() {
        for (int i = 0; i < optionPanel.transform.childCount; i++) {
            choiceButtons[i].SetActive(false);
        }
        optionPanel.SetActive(false);
        choiceSelected = null; // Forgot to reset the choiceSelected. Otherwise, it would select an option without player intervention.
        AdvanceDialogue();
    }

    /*** Tag Parser ***/
    /// In Inky, you can use tags which can be used to cue stuff in a game.
    /// This is just one way of doing it. Not the only method on how to trigger events. 
    void ParseTags() {
        tags = story.currentTags;
        foreach (string t in tags) {
            string prefix = t.Split(' ')[0];
            string param = t.Split(' ')[1];
            foreach(GameObject character in characters) {
                if (character.activeSelf) {
                    Animator animator = character.GetComponent<Animator>();
                    if (CheckAnim("TurnOnSprite", animator) || CheckAnim("FirstPopUp1", animator) || CheckAnim("FirstPopUp2", animator)) animator.SetTrigger("TurnOff");
                }
            }
            switch (prefix.ToLower()) {
                case "char":
                    SetChar(param);
                    break;
            }
        }
    }

    bool CheckAnim(string name, Animator animator) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }


    void SetChar(string _char) {
        switch (_char) {
            case "Mae":
                _nameTag = "Mãe da Kaede";
                UpdateAnimation(characters[0]);
                break;

            case "Conselheiro":
                _nameTag = "Conselheiro";
                UpdateAnimation(characters[1]);
                break;

            default:
                Debug.Log("Problema ao settar o char.");
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