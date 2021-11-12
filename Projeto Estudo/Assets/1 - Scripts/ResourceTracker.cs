using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceTracker : MonoBehaviour {

    public static int CURRENT_POPULATION;
    public static int MAX_POPULATION;
    public static int POINTS;
    public static int WAVE;
    public static int WEEK;

    [SerializeField] TextMeshProUGUI populationTMP;
    [SerializeField] TextMeshProUGUI pointsTMP;
    [SerializeField] TextMeshProUGUI MenuMessage;
    Animator MenuMessageAnimator;

    void Start() {
        MenuMessageAnimator = MenuMessage.gameObject.GetComponent<Animator>();
        MAX_POPULATION = 4;
        CURRENT_POPULATION = 0;
        POINTS = 3;
    }

    void Update() {
        populationTMP.text = CURRENT_POPULATION.ToString()+"/"+MAX_POPULATION;
        pointsTMP.text = POINTS.ToString();
    }

    public void NotEnoughPoints() {
        MenuMessage.text = "Not enough points...";
        MenuMessageAnimator.SetTrigger("MessageTrigger");
    }
    public void NotEnoughSpace() {
        MenuMessage.text = "Not enough Population...";
        MenuMessageAnimator.SetTrigger("MessageTrigger");
    }

}