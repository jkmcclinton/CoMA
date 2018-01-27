using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
//[RequireComponent(typeof(Animator))]
public class AnimController : MonoBehaviour {

    private Character character;
    private SpriteRenderer sr;
    //private Animator anim;
    private static int MOOD_TYPE_COUNT = 7;

    private Sprite[] sprites;

	// Use this for initialization
	void Start () {
        this.character = GetComponent<Character>();

        GameObject face = transform.Find("Sprites/Eyes").gameObject;
        this.sr = face.GetComponent<SpriteRenderer>();
        //this.anim = GetComponent<Animator>();
        this.sprites = Resources.LoadAll<Sprite>("Sprites/CoMA People");
	}
	
	// Update is called once per frame
	void Update () {
        // set the face sprite of the Person to reflect their mood
        int offset = 10;
        int mood = (int)(character.mood + Character.MOOD_RANGE) * (MOOD_TYPE_COUNT) / (2 * Character.MOOD_RANGE+1) + offset;
        sr.sprite = sprites[mood];
	}
}
