using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardsButton : MonoBehaviour {

    [SerializeField] GameObject buttonLeft;
    [SerializeField] GameObject buttonRight;
    [SerializeField] Transform cardsHolder;

    List<GameObject> playerCards = new List<GameObject>();

    GameObject displayCard;

    Transform[] cards = new Transform[8];
    [SerializeField] int[] cardsAmmount = new int[8];
    GameObject firstCard;
    bool multipleCards = false;
    bool hasCards;

    #region buffs
    public static bool ATTACKSPEEDDEBUFF = false;
    public static bool ATTACKSPEEDBUFF = false;
    public static bool MOVSPEEDDEBUFF = false;
    public static bool MOVSPEEDBUFF = false;
    public static bool DAMAGEDEBUFF = false;
    public static bool GLOBALDAMAGE = false;
    public static bool DAMAGEBUFF = false;
    public static bool PRICEBUFF = false;
    #endregion
    void Start() {
        cards = new Transform[8];
        cardsAmmount = new int[8];
        for (int j = 0; j < cardsHolder.childCount; j++) {
            playerCards.Add(cardsHolder.GetChild(j).gameObject);
        }
        int i = 0;
        foreach (Transform card in cardsHolder) {
            Debug.Log(cardsAmmount.Length);
            cards[i] = card;
            cardsAmmount[i] = 1;
            i++;
        }
    }

    public void ShowCards() {
        if (RemaingCard()) {
            firstCard.SetActive(true);
            displayCard = firstCard;
        }
        firstCard = null;


        foreach (GameObject card in playerCards) {
            if (card != null && firstCard != card) multipleCards = true;
        }
        if (multipleCards) {
            buttonRight.SetActive(true);
            buttonLeft.SetActive(true);
        }
        else {
            buttonRight.SetActive(false);
            buttonLeft.SetActive(false);

        }
    }


    public void NextCard() {
        GameObject nextCard = null;
        int index = 0;
        foreach (GameObject card in playerCards) {
            Debug.Log("index: " + index);
            if (card == displayCard) break;
            index++;
        }
        int _index = 0;
        foreach (GameObject card in playerCards) {
            Debug.Log("_index: " + _index);
            if (index == 7) index = -1;
            if (_index > index && card != null && displayCard != card) {
                nextCard = card;
                break;
            }
                _index++;
        }
        displayCard.SetActive(false);
        displayCard = nextCard;
        displayCard.SetActive(true);
    }

    void ActivateEffect(int index) {
        switch (index) {
            case 0:
                if (cardsAmmount[0] > 0) {
                    GLOBALDAMAGE = true;
                    cardsAmmount[0]--;
                    if (cardsAmmount[0] == 0) {
                        playerCards[0] = null;
                        ShowCards();
                    }
                }
                break;

            case 1:
                if (cardsAmmount[1] > 0) {
                    ATTACKSPEEDBUFF = true;
                    cardsAmmount[1]--;
                    if (cardsAmmount[1] == 0) {
                        playerCards[1] = null;
                        ShowCards();
                    }
                }
                break;

            case 2:
                if (cardsAmmount[2] > 0) {
                    DAMAGEDEBUFF = true;
                    cardsAmmount[2]--;
                    if (cardsAmmount[2] == 0) {
                        playerCards[2] = null;
                        ShowCards();
                    }
                }
                break;

            case 3:
                if (cardsAmmount[3] > 0) {
                    ATTACKSPEEDDEBUFF = true;
                    cardsAmmount[3]--;
                    if (cardsAmmount[3] == 0) {
                        playerCards[3] = null;
                        ShowCards();
                    }
                }
                break;

            case 4:
                if (cardsAmmount[4] > 0) {
                    PRICEBUFF = true;
                    cardsAmmount[4]--;
                    if (cardsAmmount[4] == 0) {
                        playerCards[4] = null;
                        ShowCards();
                    }
                }
                break;

            case 5:
                if (cardsAmmount[5] > 0) {
                    MOVSPEEDDEBUFF = true;
                    cardsAmmount[5]--;
                    if (cardsAmmount[5] == 0) {
                        playerCards[5] = null;
                        ShowCards();
                    }
                }
                break;

            case 6:
                if (cardsAmmount[6] > 0) {
                    MOVSPEEDBUFF = true;
                    cardsAmmount[6]--;
                    if (cardsAmmount[6] == 0) {
                        playerCards[6] = null;
                        ShowCards();
                    }
                }
                break;

            case 7:
                if (cardsAmmount[7] > 0) {
                    DAMAGEBUFF = true;
                    cardsAmmount[7]--;
                    if (cardsAmmount[7] == 0) {
                        playerCards[7] = null;
                        ShowCards();
                    }
                }
                break;
        }

        if (!RemaingCard()) {
            hasCards = false;
        }
    }

    void UpdateAmmount() {
        int i;
        for (i = 0; i < playerCards.Count; i++) {
            if (playerCards[i] == displayCard) break;
        }
        displayCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = i.ToString();
    }



    bool RemaingCard() {
        bool test = false;
        foreach (GameObject card in playerCards) {
            if (!test) firstCard = card;
            if (card != null) test = true;
        }
        return test;
    }
}
