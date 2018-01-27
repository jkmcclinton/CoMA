using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCubicles : MonoBehaviour {

	public GameObject cubicle;

	// Use this for initialization
	void Awake () {
		Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Spawn () {
		int count = 0;
		for (int h = 0; h < 3; h++) {
			for (int w = -4; w < 5; w++) {
				if (w != 0) {
					GameObject inst = Instantiate (cubicle, new Vector3 (w, 2 - 2 * h), Quaternion.identity) as GameObject;
					inst.transform.parent = GameObject.Find ("Level").transform;
					inst.name = "Cubicle" + count.ToString ();
					count++;
				}
			}
		}
	}
}
