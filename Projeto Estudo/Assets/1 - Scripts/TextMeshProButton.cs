using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProButton : MonoBehaviour {
    TextMeshProUGUI TMP;
    [SerializeField] float minSize = 1;
    [SerializeField] float maxSize = 1.2f;
    [SerializeField] Color32 unhoveredColor = new Color32(255, 255, 255, 255);
    [SerializeField] Color32 hoveredColor = new Color32(255, 255, 0, 255);
    [SerializeField] int speed = 7;
    [SerializeField] bool startMenu = true;
    Vector3 baseVector;
    bool on;
    void Start() {
        baseVector = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (startMenu) Cursor.lockState = CursorLockMode.None;
        TMP = GetComponent<TextMeshProUGUI>();
    }
    
    void OnEnable() {
        baseVector = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        transform.localScale = baseVector * minSize;
        if(TMP == null) TMP = GetComponent<TextMeshProUGUI>();
        TMP.color = unhoveredColor;
        on = false;
    }

    void Update() {
        transform.localScale = on ? Vector3.Lerp (transform.localScale, baseVector * maxSize, speed * Time.unscaledDeltaTime) 
                                  : Vector3.Lerp (transform.localScale, baseVector * minSize, speed * Time.unscaledDeltaTime);


        TMP.color = on ? Color32.Lerp (TMP.color, hoveredColor, speed * Time.unscaledDeltaTime) 
                       : Color32.Lerp (TMP.color, unhoveredColor, speed * Time.unscaledDeltaTime);


    }

    public void Hover(bool isHoveringOn) {
        //Debug.Log("Hovering over: " + transform.name);
        on = isHoveringOn;
    }
}
