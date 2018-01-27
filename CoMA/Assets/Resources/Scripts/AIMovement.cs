using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AIMovement : BaseMovement {

	public float time = 0.0f;
	private Vector3 targetPos = Vector3.zero;
	public enum AIState { Idle, Talk, Seek, Enforce };
	public AIState state;

	[HideInInspector]
	private Character character;

	// Use this for initialization
	void Start () {
		character = GetComponent<Character> ();
		switch (character.type) {
		case Character.CharacterClass.normie:
			state = AIState.Idle;
			break;
		case Character.CharacterClass.bibleThumper:
		case Character.CharacterClass.meanie:
			state = AIState.Enforce;
			break;
		default:
			break;
		}
	}

	void Update(){
		if (!canMove)
			return;

		// Update AI pathing every time Time hits 0.
		if (time > 0) {
			time -= Time.deltaTime;
		} else {
			updateAI ();
		}

		transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.10f);
	}

	void updateAI(){
		switch (state) { 
		case AIState.Idle:
			// No movement. Do nothing. Default state.
			if (state == AIState.Idle && transform.position == targetPos) {
				time = Random.Range (1.5f, 3.0f);
				state = AIState.Seek;
			}
			break;
		case AIState.Seek:
			// By default: search for nearby person. If low mood: search for nearby happy person.
			// Else: go randomly.

			// Case 1: Mood is not low. Search for a nearby person.
			// Case 2: Mood is low. Search for nearby happy person.
			// Case 3: Nobody is nearby. Move within random location.
			targetPos.x = (transform.position.x - Random.Range (-1.5f, 1.5f));
			targetPos.y = (transform.position.z - Random.Range (-1.5f, 1.5f));
			state = AIState.Idle;
			break;
		case AIState.Talk:
			// Placeholder for now.
			break;
		case AIState.Enforce:
			// Essentially an uber-seek.
			break;
		default:
			break;
		}
	}
}
