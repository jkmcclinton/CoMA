using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : MonoBehaviour {

    private CanvasController cc;

    void Start() {
        cc = FindObjectOfType<CanvasController>();
    }

	public void startHeader() {
        cc.GetComponent<Animator>().SetTrigger("EnterProgress");
    }
}
