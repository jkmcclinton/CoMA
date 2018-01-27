using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    private Image Neg, Pos;
    private Slider indicator;
    private LevelController lc;

	// Use this for initialization
	void Start () {
        Neg = transform.Find("ScoreBar/Negative").GetComponent<Image>();
        Pos = transform.Find("ScoreBar/Positive").GetComponent<Image>();
        indicator = transform.Find("ScoreBar/Indicator").GetComponent<Slider>();

        indicator.minValue = -Character.MOOD_RANGE;
        indicator.maxValue = Character.MOOD_RANGE;
        lc = GameObject.FindObjectOfType<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
        indicator.value = lc.LevelScore;
        float val = lc.LevelScore / Character.MOOD_RANGE;
        Neg.fillAmount = -Mathf.Clamp(val, -1, 0);
        Pos.fillAmount = Mathf.Clamp(val, 0, 1);
    }

    public void Notify() {
        lc.runMe();
        FindObjectOfType<PlayerMovement>().canMove = true;
    }
}
