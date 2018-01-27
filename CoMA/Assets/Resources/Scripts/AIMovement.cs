using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AIMovement : BaseMovement {

	public float time = 0.0f;
	private Vector2 targetPos = Vector2.zero;
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

		transform.position = Vector2.MoveTowards(transform.position, targetPos, 0.15f);
	}

	void updateAI(){
		Vector2 nextTarget = transform.position;
		switch (state) { 
		case AIState.Idle:
			// No movement. Do nothing. Default state.
			if (transform.position.x == targetPos.x && transform.position.z == targetPos.y) {
				// Currently at target position. Idle for 2.5 - 5 seconds, then re-enter seek mode.
				time = Random.Range (2.5f, 5.0f);
				state = AIState.Seek;
			}
			break;
		case AIState.Seek:
			// By default: search for nearby person. If low mood: search for nearby happy person.
			// Else: go randomly.
			targetPos.x = (transform.position.x - Random.Range (-5.0f, 5.0f));
			targetPos.y = (transform.position.z - Random.Range (-5.0f, 5.0f));
			time = Random.Range (2.5f, 5.0f);
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
