using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	public CharacterClass type = CharacterClass.normie;
    public LevelController level;
    public const int MOOD_RANGE = 10;
    public const int SPEED = 5;

    public SpriteRenderer Skin, Hair, Face, Clothes;

    [Range(-MOOD_RANGE, MOOD_RANGE)]
	public int mood = 0; // [-10, 10], [sad, happy]. 0 = Neutral
    [Range(-MOOD_RANGE, MOOD_RANGE)]
    public int defaultMood = 0;
    
	public float secondsBetweenRegens = 10f;
	public float secondsElapsed = 0f;

	public float bibleBombMaxTime = 10f;
	public float bibleBombTime = 0f;

	public bool beingInfected = false;
    private Sprite[] sprites;
    private TrailRenderer trail;
	public LayerMask npcLayer;

    public virtual void Awake () {
        Skin = transform.Find("Sprites/Skin").GetComponent<SpriteRenderer>();
        Hair = transform.Find("Sprites/Hair").GetComponent<SpriteRenderer>();
        Face = transform.Find("Sprites/Eyes").GetComponent<SpriteRenderer>();
        Clothes = transform.Find("Sprites/Clothes").GetComponent<SpriteRenderer>();

        this.level = GameObject.FindObjectOfType<LevelController>();
        this.sprites = Resources.LoadAll<Sprite>("Sprites/CoMA People");
        this.trail = Clothes.GetComponent<TrailRenderer>();
	}

     void Start() {

        //StartCoroutine(RegenMood());
    }

    public virtual void Update () {

		if (type == CharacterClass.bibleThumper) {

			if (bibleBombTime < bibleBombMaxTime) {
				bibleBombTime += Time.deltaTime;
			} else {
				//print ("BIBLE BOMB!");
				Collider2D[] NPCs = Physics2D.OverlapCircleAll((Vector2)transform.position, 2f, npcLayer);

				//print (NPCs.Length);
				foreach (var npc in NPCs) {
				//	print ("Affected: " + npc.name);
					npc.transform.parent.GetComponent<Character> ().mood += 1;
					npc.transform.parent.GetComponent<Character> ().mood = (int)Mathf.Clamp(npc.transform.parent.GetComponent<Character> ().mood, -10f, 10f);
				}

				bibleBombTime = 0f;
			}


		}

	}

    public void BecomeEnforcer(bool isDep) {
        if(isDep) {
			mood = -10;
			defaultMood = -10;
            Skin.sprite = sprites[24];
            Hair.sprite = sprites[4];
            Clothes.sprite = sprites[9];
            trail.startColor = new Color(0, 7 / 255f, 141 / 255f, 1);
            trail.endColor = new Color(0, 7 / 255f, 141 / 255f, 0);
            trail.enabled = true;
        } else {
			mood = 10;
			defaultMood = 10;
            Skin.sprite = sprites[24];
            Hair.sprite = sprites[4];
            Clothes.sprite = sprites[9];
            trail.startColor = new Color(241 / 255f, 255 / 255f, 0 / 255f, 1);
            trail.endColor = new Color(134 / 255f, 7 / 203, 3 / 255f, 0);
            trail.enabled = true;
        }
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
			yield return new WaitForSeconds (1f);
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
			yield return new WaitForSeconds (5f);
			if (!beingInfected)
				break;

			if (Mathf.Abs (mood) < MOOD_RANGE) {
				mood += (decrement ? -1 : 1);
			}

			Collider2D[] FriendsNear = Physics2D.OverlapCircleAll (transform.position, FindObjectOfType<Infect>().aoeRadius, LayerMask.GetMask("Player"));
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