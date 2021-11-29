using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public int health;
    public int points;
    public int pop;
    public int maxPop;
    public bool[] unitsList;
    public int week;
    public int[] cards;



    public PlayerData(int health, int points, int pop, int maxPop, bool[] unitsList, int week, int[] cards) {
        this.health = health;
        this.points = points;
        this.pop = pop;
        this.maxPop = maxPop;
        this.unitsList = unitsList;
        this.week = week;
        this.cards = cards;
    }


}
