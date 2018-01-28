using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public CharacterClass type = CharacterClass.normie;
    public LevelController level;
    public const int MOOD_RANGE = 10;
    public const int SPEED = 5;

    [Range(-MOOD_RANGE, MOOD_RANGE)]
	public int mood = 0; // [-10, 10], [sad, happy]. 0 = Neutral
    [Range(-MOOD_RANGE, MOOD_RANGE)]
    public int defaultMood = 0;
    
	public float secondsBetweenRegens = 10f;
	public float secondsElapsed = 0f;

	public bool beingInfected = false;

	public virtual void Start () {
//		switch (type) {
//		case CharacterClass.normie:
//			defaultMood = 0;
//			secondsBetweenRegens = 10f;
//			break;
//		case CharacterClass.player:
//			defaultMood = -10;
//			secondsBetweenRegens = 10f;
//			break;
//		case CharacterClass.bibleThumper:
//			defaultMood = 10;
//			secondsBetweenRegens = 0.1f;
//			break;
//		case CharacterClass.meanie:
//			defaultMood = -10;
//			secondsBetweenRegens = 0.1f;
//			break;
//		default:
//			print ("Error: Character Class not recognized.");
//		}
			
        this.level = GameObject.FindObjectOfType<LevelController>();

		StartCoroutine (RegenMood ());
	}

	public virtual void Update () {

	}

	public IEnumerator RegenMood () {
		while (true) {
			secondsElapsed = 0f;
			while (secondsElapsed < secondsBetweenRegens) {
				if (!beingInfected) {
					secondsElapsed += Time.deltaTime;
					yield return new WaitForEndOfFrame ();
				} else {
					secondsElapsed = 0f;
					yield return new WaitForEndOfFrame ();
				}
			}

			if (mood != defaultMood) {
				mood += (int)Mathf.Sign (defaultMood - mood);

                //level.TallyAgonyConversion();
                //level.TallyJoyConversion();
			}
		}
	}

	public virtual void Infect (bool active, bool decrement) {
		//print ("Infect Called! Active: " + active + "\tDecrement: " + decrement);
		if (active) {
			if (!beingInfected) {
				StartCoroutine (Infect (decrement));
			}
		} else {
			StopCoroutine ("Infect");
			beingInfected = false;
			GetComponent<BaseMovement> ().canMove = true;
		}
	}

	public virtual void InfectAoE (bool active, bool decrement) {
		if (active) {
			if (!beingInfected) {
				StartCoroutine (InfectAoE (decrement));
			}
		} else {
			StopCoroutine ("InfectAoE");
			beingInfected = false;
			GetComponent<BaseMovement> ().canMove = true;
		}
	}

	public IEnumerator Infect (bool decrement) {
		beingInfected = true;
		GetComponent<BaseMovement> ().canMove = false;

		while (true) {
			yield return new WaitForSeconds (2f);
			if (!beingInfected)
				break;

			if (Mathf.Abs (mood) < MOOD_RANGE) {
				mood += (decrement ? -1 : 1);
			}
		}
	}

	public IEnumerator InfectAoE (bool decrement) {
		beingInfected = true;

		while (true) {
			// Instant effect - target mood +- 2.
			yield return new WaitForSeconds (0f);
			if (!beingInfected)
				break;

			if (Mathf.Abs (mood) < MOOD_RANGE) {
				mood += (decrement ? -2 : 2);
			}
		}
	}

	public enum CharacterClass
	{
		player, bibleThumper, normie, meanie
	}


	public virtual void OnGUI () {
		//Vector2 origin = transform.position;
		//GUI.TextField(new Rect((Vector2)transform.position, Vector2.one * 20), mood.ToString());
	}

}