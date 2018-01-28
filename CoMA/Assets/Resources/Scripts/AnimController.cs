using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AnimController : MonoBehaviour {

    private Character character;
    private Rigidbody2D body;
    private Animator anim;
    private static int MOOD_TYPE_COUNT = 7;

    private Sprite[] sprites;



	// Use this for initialization
	void Start () {
        this.character = GetComponent<Character>();
        this.body = GetComponent<Rigidbody2D>();

        this.anim = GetComponent<Animator>();
        this.sprites = Resources.LoadAll<Sprite>("Sprites/CoMA People");
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", Mathf.Abs(body.velocity.magnitude) > .01f );
		if (body.velocity.x < -Time.deltaTime) {
			character.Face.flipX = true;
            character.Hair.flipX = true;
            character.Skin.flipX = true;
            character.Clothes.flipX = true;
		} else if (body.velocity.x > Time.deltaTime) {
            character.Face.flipX = false;
            character.Hair.flipX = false;
            character.Skin.flipX = false;
            character.Clothes.flipX = false;
		}
        
        // set the face sprite of the Person to reflect their mood
        int offset = 10;
        int mood = (int)(character.mood + Character.MOOD_RANGE) * (MOOD_TYPE_COUNT) / (2 * Character.MOOD_RANGE+1) + offset;
		if (character.Face.sprite != sprites [mood]) {
			if (tag != "Player" && GetComponent<Character>().beingInfected) {
				ParticleEffect ();
			}
		}

        character.Face.sprite = sprites[mood];
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Character"))
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), 
                GetComponent<Collider2D>());
    }

	private void ParticleEffect () {
		GetComponent<ParticleSystem> ().Play ();
	}
}
