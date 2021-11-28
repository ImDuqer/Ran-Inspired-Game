using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PulaByobo : MonoBehaviour
{
    [SerializeField] GameObject circle;
    [SerializeField] int scene = 2; 
    float scale;
    float maxScale;
    bool load = false;

    void Start() {
        maxScale = circle.transform.localScale.x;
        scale = 0;
    }

    void Update() {
        if (!load) {
            if (Input.anyKey) {
                scale += Time.deltaTime * 0.5f;
                if (scale >= maxScale) {
                    SceneManager.LoadScene(scene);
                    load = true;
                }
            }
            else {
                if (scale > 0) scale -= Time.deltaTime;
                else scale = 0;
            }
            circle.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}
