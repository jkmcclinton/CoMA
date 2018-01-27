using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public CharacterClass type = CharacterClass.bibleThumper;
    public LevelController level;
    public const int MOOD_RANGE = 10;
    
    [Range(-MOOD_RANGE, MOOD_RANGE)]
	public int mood = 0; // [-10, 10], [sad, happy]. 0 = Neutral
    [Range(-MOOD_RANGE, MOOD_RANGE)]
    public int defaultMood = 0;
    
	public float secondsBetweenRegens = 10f;
	public float secondsElapsed = 0f;

	public bool beingInfected = false;

	public virtual void Start () {
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
		}
	}

	public IEnumerator Infect (bool decrement) {
		beingInfected = true;

		while (true) {
			yield return new WaitForSeconds (2f);
			if (!beingInfected)
				break;

			if (Mathf.Abs (mood) < MOOD_RANGE) {
				mood += (decrement ? -1 : 1);
			}
		}
	}

	public enum CharacterClass
	{
		bibleThumper, fatty, bolemic, shwifty, nifty, sugar, spice, everythingNice
	}

}