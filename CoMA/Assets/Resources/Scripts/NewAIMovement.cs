using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAIMovement : BaseMovement {

	[Header("Random Ranges")]
	public Vector2 idleRange = new Vector2(1f, 10f);
	public Vector2 distanceRange = new Vector2(5f, 20f);

	[Header("Data")]
	public float speed = 5f;
	public float idleTime = 0f;
	public float timeWaited = 0f;
	public float distance = 0f;
	public float distanceTravelled = 0f;
	public int direction = 1;

	// Use this for initialization
	void Start () {
		StartCoroutine (Move ());
	}
	
	// Update is called once per frame
	void Update () {



//		if ((Mathf.Abs (distance) - Mathf.Abs (distanceTravelled)) < speed * Time.deltaTime) {
//
//		}
//
//		if (Mathf.Abs (idleTime - timeWaited) < Time.deltaTime) {
//			NewMove ();
//		} else {
//			timeWaited += Time.deltaTime;
//		}
			


	}

	public IEnumerator Move () {
		while (true) {
			timeWaited = 0f;
			idleTime = Random.Range (idleRange.x, idleRange.y);

			distanceTravelled = 0f;
			distance = Random.Range (distanceRange.x, distanceRange.y);

			direction = Random.Range (0, 2) * 2 - 1;

			while ((Mathf.Abs (distance) - Mathf.Abs (distanceTravelled)) > speed * Time.deltaTime) {
				if (!canMove) {
					StopCoroutine (Move ());
				}

				transform.position += new Vector3 (direction * speed * Time.deltaTime, 0f);
				distanceTravelled += speed * Time.deltaTime;
				yield return new WaitForEndOfFrame ();
			}

			while (timeWaited < idleTime) {
				if (!canMove) {
					StopCoroutine (Move ());
				}
				timeWaited += Time.deltaTime;
				yield return new WaitForEndOfFrame ();
			}

			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator Wait () {
		yield return new WaitForSeconds (idleTime);
	}
}
