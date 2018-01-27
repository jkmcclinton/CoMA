using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    private Slider slider;
    private LevelController lc;

	// Use this for initialization
	void Start () {
        slider = transform.Find("ScoreBar").GetComponent<Slider>();
        slider.minValue = -Character.MOOD_RANGE;
        slider.maxValue = Character.MOOD_RANGE;
        lc = GameObject.FindObjectOfType<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = lc.LevelScore;
	}
}
