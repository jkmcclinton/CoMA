using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMovement {

	public LayerMask groundMask;

	// Checks every frame.
	void Update () {
		if (!canMove)
			return;
		
		var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0).normalized * 7;

		//transform.position += move * Time.deltaTime;

		if (Input.GetKeyDown (KeyCode.Space)) {
			if (Physics2D.Raycast (transform.position, Vector2.down, 0.55f, groundMask)) {
				GetComponent<Rigidbody2D> ().AddForce (Vector2.up * 700);
			}
		}
	}
}
