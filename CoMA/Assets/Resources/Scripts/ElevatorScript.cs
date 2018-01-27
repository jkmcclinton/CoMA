using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {

	private bool touchingElevator = false;
	private Animator anim;

	void Start(){
		anim = GetComponent<Animator> ();
	}

	void Update () {
		// Player activates elevator.
		if ((Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && touchingElevator) {
			Debug.Log ("Moving up!");
			anim.SetTrigger ("Elevator");
			TransportPlayer ();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			touchingElevator = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			touchingElevator = false;
		}
	}

	void TransportPlayer() {
		GameObject.FindGameObjectWithTag ("Player").transform.position;
	}
}
