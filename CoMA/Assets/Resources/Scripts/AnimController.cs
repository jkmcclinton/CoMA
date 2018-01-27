using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class AnimController : MonoBehaviour {

    private Character character;
    private SpriteRenderer face, hair, skin, clothes;
    private Rigidbody2D body;
    private Animator anim;
    private static int MOOD_TYPE_COUNT = 7;

    private Sprite[] sprites;

	// Use this for initialization
	void Start () {
        this.character = GetComponent<Character>();
        this.body = GetComponent<Rigidbody2D>();
        GameObject f = transform.Find("Sprites/Eyes").gameObject;
        GameObject h = transform.Find("Sprites/Hair").gameObject;
        GameObject s = transform.Find("Sprites/Skin").gameObject;
        GameObject c = transform.Find("Sprites/Clothes").gameObject;
        this.face = f.GetComponent<SpriteRenderer>();
        this.hair = h.GetComponent<SpriteRenderer>();
        this.skin = s.GetComponent<SpriteRenderer>();
        this.clothes = c.GetComponent<SpriteRenderer>();
        this.anim = GetComponent<Animator>();
        this.sprites = Resources.LoadAll<Sprite>("Sprites/CoMA People");
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetBool("Moving", Mathf.Abs(body.velocity.magnitude) > .01f );
        face.flipX = body.velocity.x < 0;
        hair.flipX = body.velocity.x < 0;
        skin.flipX = body.velocity.x < 0;
        clothes.flipX = body.velocity.x < 0;
        
        // set the face sprite of the Person to reflect their mood
        int offset = 10;
        int mood = (int)(character.mood + Character.MOOD_RANGE) * (MOOD_TYPE_COUNT) / (2 * Character.MOOD_RANGE+1) + offset;
        face.sprite = sprites[mood];
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Character"))
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), 
                GetComponent<Collider2D>());
    }
}
