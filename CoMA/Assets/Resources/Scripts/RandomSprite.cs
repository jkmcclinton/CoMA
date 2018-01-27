using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour {

    public Sprite First, Last;

    private SpriteRenderer sr;
    private List<Sprite> sprites;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>("Sprites").ToList();
        if(First && Last != null) {
            int f = sprites.IndexOf(First);
            int l = sprites.IndexOf(Last);
            int i = /*(f != l) ?*/ (l > f ? Random.Range(l, f) : Random.Range(f, l)) /*: l*/;
            sr.sprite = sprites[i];
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
