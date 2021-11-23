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

    [HideInInspector] public Transform[] cards = new Transform[8];
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
    void Awake() {
        cards = new Transform[8];
        cardsAmmount = new int[8];
        for (int j = 0; j < cardsHolder.childCount; j++) {
            playerCards.Add(cardsHolder.GetChild(j).gameObject);
        }
        int i = 0;
        foreach (Transform card in cardsHolder) {
            //Debug.Log(cardsAmmount.Length);
            cards[i] = card;
            cardsAmmount[i] = 1;
            i++;
        }
    }

    void Update() {
        //foreach (GameObject card in playerCards) {
        //    Debug.Log("card: " + card);
        //}
    }

    public void ShowCards() {
        if (RemaingCard()) {
            firstCard.SetActive(true);
            displayCard = firstCard;
        }
        firstCard = null;


        foreach (GameObject card in playerCards) {
            Debug.Log("card: " + card);
            if (card != null && displayCard != card) multipleCards = true;
            else multipleCards = false;
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
            //Debug.Log("index: " + index);
            if (card == displayCard) break;
            index++;
        }

        int _index = 0;
        if (index == playerCards.Count-1) index = -1;

        for (int i = 0; i < playerCards.Count; i++) {
            GameObject card = playerCards[i];
            if (_index > index && card != null && displayCard != card) {
                nextCard = card;
                break;
            }
            _index++;
            if (i == playerCards.Count && nextCard == null) {
                index = -1;
                i = 0;
            }
        }
        displayCard.SetActive(false);
        displayCard = nextCard;
        displayCard.SetActive(true);
    }


    public void PreviousCard() {
        GameObject previousCard = null;
        int index = 0;
        foreach (GameObject card in playerCards) {
            //Debug.Log("index: " + index);
            if (card == displayCard) break;
            index++;
        }
        int _index = 7;
        if (index == 0) index = playerCards.Count;

        for (int i = playerCards.Count-1; i >= 0; i-- ) {
            GameObject card = playerCards[i];
            //Debug.Log("_index: " + _index);
            if (_index < index && card != null && displayCard != card) {
                previousCard = card;
                break;
            }
            _index--;
            if (i == 0 && previousCard == null) {
                index = playerCards.Count;
                i = playerCards.Count - 1;
            }
        }

        displayCard.SetActive(false);

        displayCard = previousCard;
        Debug.Log(previousCard);
        displayCard.SetActive(true);

    }

    public void ActivateEffect(int index) {
        switch (index) {
            case 0:
                GLOBALDAMAGE = UseEffect(0);
                break;

            case 1:
                ATTACKSPEEDBUFF = UseEffect(1);
                break;

            case 2:
                DAMAGEDEBUFF = UseEffect(2);
                break;

            case 3:
                ATTACKSPEEDDEBUFF = UseEffect(3);
                break;

            case 4:
                PRICEBUFF = UseEffect(4);
                break;

            case 5:
                MOVSPEEDDEBUFF = UseEffect(5);
                break;

            case 6:
                MOVSPEEDBUFF = UseEffect(6);
                break;

            case 7:
                DAMAGEBUFF = UseEffect(7);
                break;
        }
        UpdateAmmount();
        if (!RemaingCard()) {
            hasCards = false;
        }
    }

    bool UseEffect(int index) {
        if (cardsAmmount[index] > 0) {
            cardsAmmount[index]--;
            if (cardsAmmount[index] == 0) {
                int i = 0;
                foreach (GameObject card in playerCards) {
                    if (card == cards[index].gameObject) break;
                    i++;
                }
                playerCards[i].SetActive(false);
                playerCards[i] = null;
                playerCards.Remove(playerCards[i]);
                ShowCards();
            }
            return true;
        }
        return false;
    }


    void UpdateAmmount() {
        int i;
        for (i = 0; i < playerCards.Count; i++) {
            cards[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = cardsAmmount[i].ToString();
        }
        //s displayCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = i.ToString();
    }


    public void BuyCard(GameObject go) {
        int i;
        i = go.GetComponent<CardChoice>().index;
        cardsAmmount[i]++;
        if (cardsAmmount[i] == 1) playerCards[i] = cards[i].gameObject;
        UpdateAmmount();
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
