using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour{

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
		Debug.Log ("PlayerMovement loaded successfully.");
	}

	// Checks every frame.
	void Update () {
		var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical")).normalized * Character.SPEED *2;
        //transform.position += move * Time.deltaTime;
        body.AddForce(move);
	}
}
