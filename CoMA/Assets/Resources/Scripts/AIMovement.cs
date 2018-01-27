using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : BaseMovement {

	public float time = 0.0f;
	public float timePeriod = 5.0f;
	private Vector3 move = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		Debug.Log ("AIMovement loaded successfully.");
		move.x = Random.Range(-5.0f, 5.0f);
	}

	void Update(){
		if (!canMove)
			return;

		time += Time.deltaTime;

		if (time > timePeriod) {
			time = 0.0f;
			move.x = Random.Range(-5.0f, 5.0f);
			Debug.Log (move.x);
		}

		transform.position += move * Time.deltaTime;
	}
}
