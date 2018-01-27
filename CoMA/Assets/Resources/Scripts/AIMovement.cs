using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : BaseMovement {

	public float time = 0.0f;
	public float timePeriod = 3.0f;
	private Vector3 move = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		move.x = Random.Range(-2.0f, 2.0f);
	}

	void Update(){
		if (!canMove)
			return;

		time += Time.deltaTime;

		if (time > timePeriod) {
			time = 0.0f;
//			Debug.Log (move.x);
		}

		//GetComponent<Rigidbody2D> ().velocity = move;
		//transform.position += move * Time.deltaTime;
	}
}
