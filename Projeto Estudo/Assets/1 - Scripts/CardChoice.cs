using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardChoice : MonoBehaviour
{
    public CardsButton copyFrom;
    Transform[] choiceOfCards;
    Image myImage;
    [HideInInspector] public int index;

    TextMeshProUGUI description;
    void OnEnable() {
        description = transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>();
        myImage = GetComponent<Image>();
        choiceOfCards = copyFrom.cards;
        index = Random.Range(0, choiceOfCards.Length - 1);
        Debug.Log(index);
        Debug.Log(choiceOfCards[index]);
        myImage.sprite = choiceOfCards[index].gameObject.GetComponent<Image>().sprite;
        description.text = choiceOfCards[index].gameObject.transform.GetChild(0).GetChild(2).GetComponent<TextMeshProUGUI>().text;
    }

}
