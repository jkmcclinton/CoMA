using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

	//private Rigidbody2D playerBody;

	// Use this for initialization
	void Start () {
		Debug.Log ("PlayerMovement loaded successfully.");
	}

	// Checks every frame.
	void Update () {
		var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		transform.position += move * Time.deltaTime;
	}
}
