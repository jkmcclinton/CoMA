using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour {

	public static int instance = 0;

	// Use this for initialization
	void Start () {
		instance++;
		foreach (Transform child in transform) {
			child.GetComponent<SpriteRenderer> ().sortingOrder += 100 * instance;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
