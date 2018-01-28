using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infect : MonoBehaviour {

	public GameObject target;
	public float infectRadius = 1f;

	public Collider2D[] targets;

	public LayerMask infectMask;
	public bool infecting = false;
	public string infectingSpeech = "";
	private int lastSpeech = -1;

	// Use this for initialization
	void Start () {
		GetRandomSpeech ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		targets = Physics2D.OverlapCircleAll (transform.position, infectRadius, infectMask);

		if (Input.GetButton ("CloseAttack")) {
			float minDist = Mathf.Infinity;

			//If there is already a target, don't find a new one
			if (!target) {
				foreach (var item in targets) {
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
		} else {
			if (target) {
				GetComponent<PlayerMovement> ().canMove = true;
				target.transform.parent.GetComponent<Character> ().Infect (false, true);
				target = null;
				infecting = false;
				CancelInvoke ("GetRandomSpeech");
			}
		}

		if (Input.GetButton ("AoEAttack")) {
			Debug.Log ("AoE attack!");
			Invoke ("GetRandomSpeech", 0f);
			if (!target) {
				target.transform.parent.GetComponent<Character> ().Infect (true, true);
				infecting = true;
			}
		}
	}

	void GetRandomSpeech () {
		int val = Random.Range (0, 13);

		while (val == lastSpeech) {
			val = Random.Range (0, 13);
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
