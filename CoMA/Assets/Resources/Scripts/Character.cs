using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public CharacterClass type = CharacterClass.bibleThumper;

	public int mood = 0; // [-10, 10], [sad, happy]. 0 = Neutral
	public int defaultMood = 0;

	public float secondsBetweenRegens = 10f;
	public float secondsElapsed = 0f;

	public bool moodChanging = false;

	public virtual void Start () {
		StartCoroutine (RegenMood ());
	}

	public virtual void Update () {

	}

	public IEnumerator RegenMood () {
		while (true) {
			secondsElapsed = 0f;
			while (secondsElapsed < secondsBetweenRegens) {
				secondsElapsed += Time.deltaTime;
				yield return new WaitForEndOfFrame ();
			}

			if (mood != defaultMood) {
				mood += (int)Mathf.Sign (defaultMood - mood);
			}
		}
	}

	public abstract void Infect ();


	public enum CharacterClass
	{
		bibleThumper, fatty, bolemic, shwifty, nifty, sugar, spice, everythingNice
	}

}