using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : BaseMovement {

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}

	// Checks every frame.
	void FixedUpdate () {
		var move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Character.SPEED;
        //transform.position += move * Time.deltaTime;
        //body.AddForce(move);
		if (canMove)
			body.velocity = move;
		else
			body.velocity = Vector2.zero;
	}
}

//public class PlayerMovement : BaseMovement {
//
//	public LayerMask groundMask;
//
//	// Checks every frame.
//	void Update () {
//		if (!canMove)
//			return;
//
//		var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized * 7;
//
//		//transform.position += move * Time.deltaTime;
//
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			if (Physics2D.Raycast (transform.position, Vector2.down, 0.55f, groundMask)) {
//				GetComponent<Rigidbody2D> ().AddForce (Vector2.up * 700);
//			}
//		}
//	}
//}