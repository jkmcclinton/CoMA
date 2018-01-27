using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour {

    private Animator anim;
    private LevelController lc;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        lc = GameObject.FindObjectOfType<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Notify() {
        lc.notify();
    }
    public void FadeIn() { FadeIn(1);  }
    public void FadeOut() { FadeOut(1); }
    public void FadeIn(float speed) { anim.SetTrigger("FadeIn"); anim.SetFloat("FadeSpeed", 1/speed); }
    public void FadeOut(float speed) { anim.SetTrigger("FadeOut"); anim.SetFloat("FadeSpeed", 1/speed); }
}
