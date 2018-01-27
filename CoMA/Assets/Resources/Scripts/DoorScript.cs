using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

	private bool touchingDoor = false;
	private Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	void Update () {
		// Player activates elevator.
		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && touchingDoor) {
			Debug.Log ("Moving up!");
			anim.SetTrigger ("Door");
			TransportPlayer ();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			touchingDoor = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			touchingDoor = false;
		}
	}

	void TransportPlayer() {
		//GameObject.FindGameObjectWithTag ("Player").transform.position;
	}
}
