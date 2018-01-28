using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicCrossfade : MonoBehaviour {

	[Range(-10, 10)] public int happyMeter = 0;
	public AudioMixer mixer;

	public AudioClip[] neutralLeadSynthClips;
	public AudioClip[] sadLeadSynthClips;
	public AudioClip[] happyLeadSynthClips;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (happyMeter < -3) {
			mixer.SetFloat ("NAudioVol", -80f);
			mixer.SetFloat ("NDrumsVol", -80f);
			mixer.SetFloat ("DAudioVol", 0f);
			mixer.SetFloat ("DDrumsVol", 0f);
			mixer.SetFloat ("HAudioVol", -80f);
			mixer.SetFloat ("HDrumsVol", -80f);
		} else if (happyMeter < 4) {
			mixer.SetFloat ("NAudioVol", 1f);
			mixer.SetFloat ("NDrumsVol", 1f);
			mixer.SetFloat ("DAudioVol", -80f);
			mixer.SetFloat ("DDrumsVol", -80f);
			mixer.SetFloat ("HAudioVol", -80f);
			mixer.SetFloat ("HDrumsVol", -80f);
		} else {
			mixer.SetFloat ("NAudioVol", -80f);
			mixer.SetFloat ("NDrumsVol", -80f);
			mixer.SetFloat ("DAudioVol", -80f);
			mixer.SetFloat ("DDrumsVol", -80f);
			mixer.SetFloat ("HAudioVol", 1f);
			mixer.SetFloat ("HDrumsVol", 1f);
		}
	}
}
