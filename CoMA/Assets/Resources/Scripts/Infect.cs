﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infect : MonoBehaviour {

	public GameObject target;
	public float infectRadius = 1f;
	public float aoeRadius = 10f;

	public Collider2D[] normalInfectTargets;
	public Collider2D[] aoeInfectTargets;

	public LayerMask infectMask;
	public bool infecting = false;
	public string infectingSpeech = "";
	private int lastSpeech = -1;

	// Use this for initialization
	void Start () {
		GetRandomSpeech ();
	}

	void CloseAttack () {
		if (Input.GetButton ("CloseAttack")) {
			float minDist = Mathf.Infinity;

			//If there is already a target, don't find a new one
			if (!target) {
				foreach (var item in normalInfectTargets) {
					if ((item.transform.position - transform.position).sqrMagnitude < minDist) {
						target = item.gameObject;
					}
				}
			} else {
				GetComponent<PlayerMovement> ().canMove = false;
				target.transform.parent.GetComponent<Character> ().Infect (true, true);
				if (infecting == false)
					InvokeRepeating ("GetRandomSpeech", 0f, 2f);
				infecting = true;
			}
		} 
		if (Input.GetButtonUp("CloseAttack")) {
			if (target) {
				GetComponent<PlayerMovement> ().canMove = true;
				target.transform.parent.GetComponent<Character> ().Infect (false, true);
				target = null;
				infecting = false;
				CancelInvoke ("GetRandomSpeech");
			}
		}
	}

	void AoEAttack () {
		if (Input.GetButton ("AoEAttack")) {

			//If there is already a target, don't find a new one
			if (aoeInfectTargets.Length > 0) {
				foreach (var item in aoeInfectTargets) {
					item.transform.parent.GetComponent<Character> ().InfectAoE (true, true);
				}
			} else {
				//GetComponent<PlayerMovement> ().canMove = false;

				if (infecting == false)
				//	InvokeRepeating ("GetRandomSpeech", 0f, 2f);
				infecting = true;
			}
		} 
		if (Input.GetButtonUp("AoEAttack")) {
			if (target) {
				//GetComponent<PlayerMovement> ().canMove = true;
				target.transform.parent.GetComponent<Character> ().InfectAoE (false, true);
				target = null;
				infecting = false;
				//CancelInvoke ("GetRandomSpeech");
			}
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		normalInfectTargets = Physics2D.OverlapCircleAll (transform.position, infectRadius, infectMask);
		aoeInfectTargets = Physics2D.OverlapCircleAll (transform.position, aoeRadius, infectMask);

		CloseAttack ();

		AoEAttack ();
	}

	void GetRandomSpeech () {
		int val = Random.Range (0, 23);

		while (val == lastSpeech) {
			val = Random.Range (0, 23);
		}

		switch (val) {
		case 0:
			infectingSpeech = "I saw a dead bunny by the side of\nthe road earlier.";
			break;
		case 1:
			infectingSpeech = "When we die we don't get a second chance.";
			break;
		case 2:
			infectingSpeech = "Think of all the mistakes you've\nmade in your life.";
			break;
		case 3:
			infectingSpeech = "Not everyone gets to say goodbye\nto the people they love.";
			break;
		case 4:
			infectingSpeech = "Only a few restaurants I like deliver.";
			break;
		case 5:
			infectingSpeech = "People die every day.";
			break;
		case 6:
			infectingSpeech = "At some point someone will\nsay your name for the last time.";
			break;
		case 7:
			infectingSpeech = "I want Chik-Fil-A but it's Sunday.";
			break;
		case 8:
			infectingSpeech = "You will never escape your cubicle.";
			break;
		case 9:
			infectingSpeech = "The Jersey Shore was a wildly\nprofitable show.";
			break;
		case 10:
			infectingSpeech = "Did you know that the average pay raise\nno longer accounts for cost of living increases?";
			break;
		case 11:
			infectingSpeech = "Why are we here? Just to suffer?";
			break;
		case 12:
			infectingSpeech = "Do you think God stays in heaven\nbecause he, too, lives in fear of what he's created?";
			break;
		case 13:
			infectingSpeech = "Creating this game is depressing.";
			break;
		case 14:
			infectingSpeech = "Echidnas only live up to 18 years.";
			break;
		case 15:
			infectingSpeech = "Cotton-Eye Joe is probably dead.";
			break;
		case 16:
			infectingSpeech = "Fred from Youtube's last post was\nOver 2 years ago.";
			break;
		case 17:
			infectingSpeech = "Shark are killed for their fins\nEvery day.";
			break;
		case 18:
			infectingSpeech = "Mayonnaise isn't classified as an instrument.";
			break;
		case 19:
			infectingSpeech = "Rebecca farted earlier.";
			break;
		case 20:
			infectingSpeech = "My Panera Bread gift card expired yesterday.";
			break;
		case 21:
			infectingSpeech = "Amazon Prime charges for deliveries. :(";
			break;
		case 22:
			infectingSpeech = "I just got an email that started with\n'Thank you for applying...'";
			break;
		default:
			infectingSpeech = "Shoutout to SGD@UVA for the dank\nmemes and good-looking members ;)";
			break;
		}
		lastSpeech = val;
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = new Color (1, 0, 0, 0.2f);
		Gizmos.DrawSphere (transform.position, infectRadius);
	}

	void OnGUI () {
		if (infecting) {
			GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 500, 100), infectingSpeech);
		}
	}
}
