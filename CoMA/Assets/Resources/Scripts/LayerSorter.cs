using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour {

//	public static int instance = 0;
//
//	void Start () {
//		if (instance == 0) {
//			CreateSortingLayers ();
//		}
//		instance++;
//	}
//
//	void CreateSortingLayers() {
//		//SortingLayer.
//	}

	public static int instance = 0;

	private int offset;
	private int baseSortingOrder;

	// Use this for initialization
//	void Start () {
//		if (tag != "Player") {
//			instance++;
//			foreach (Transform child in transform) {
//				child.GetComponent<SpriteRenderer> ().sortingOrder -= 100 + 5 * instance;
//			}
//			baseSortingOrder = transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder;
//		} else {
//			foreach (Transform child in transform) {
//				child.GetComponent<SpriteRenderer> ().sortingOrder -= 10;
//			}
//			baseSortingOrder = transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingOrder;
//		}
//	}

	void Update () {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = (-10 * Mathf.RoundToInt (200 * transform.position.y) - i);
		}
	}
	
//	// Update is called once per frame
//	void Update () {
//		if (transform.position.y < -1.9f) {
//			offset = 0;
//		} else if (transform.position.y < 0.1f) {
//			offset = 1;
//		} else if (transform.position.y < 2.1f) {
//			offset = 2;
//		} else {
//			offset = 3;
//		}
//
//		//GetComponent<SpriteRenderer> ().sortingOrder = baseSortingOrder - offset * 200;
//		for (int i = 0; i < transform.childCount; i++) {
//			transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = baseSortingOrder - i - offset * 200;
//		}
//	}
}
