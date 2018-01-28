using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    private Image Neg, Pos;
    private Slider indicator;
    private LevelController lc;
    private Text timer;

	// Use this for initialization
	void Start () {
		Neg = transform.Find("ScoreBar/Panel/Negative").GetComponent<Image>();
		Pos = transform.Find("ScoreBar/Panel/Positive").GetComponent<Image>();
		indicator = transform.Find("ScoreBar/Panel/Indicator").GetComponent<Slider>();
        timer = transform.Find("ScoreBar/Timer").GetComponent<Text>();

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
        timer.text = LevelController.formatTime(lc.time);
    }

    public void Notify() {
        lc.runMe();

        /// unlock game
        FindObjectOfType<PlayerMovement>().canMove = true;
        AIMovement[] NPCs = GameObject.Find("NPCs").transform.GetComponentsInChildren<AIMovement>();
        foreach (AIMovement NPC in NPCs) NPC.canMove = true;
    }

    public void EG() {
        lc.EndGame();
    }
}
