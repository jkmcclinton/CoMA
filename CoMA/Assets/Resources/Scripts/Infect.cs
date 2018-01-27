using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : MonoBehaviour {

	public GameObject target;
	public float infectRadius = 1f;

	public Collider2D[] targets;

	public LayerMask infectMask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		targets = Physics2D.OverlapCircleAll (transform.position, infectRadius, infectMask);

		if (Input.GetKey (KeyCode.E)) {
			float minDist = Mathf.Infinity;

			//If there is already a target, don't find a new one
			if (!target) {
				foreach (var item in targets) {
					if ((item.transform.position - transform.position).sqrMagnitude < minDist) {
						target = item.gameObject;
					}
				}
			} else {
				target.GetComponent<Character> ().Infect (true, true); //{

				//}
			}
		} else {
			if (target) {
				target.GetComponent<Character> ().Infect (false, true);
				target = null;
			}
		}
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = new Color (1, 0, 0, 0.2f);
		Gizmos.DrawSphere (transform.position, infectRadius);
	}
}
