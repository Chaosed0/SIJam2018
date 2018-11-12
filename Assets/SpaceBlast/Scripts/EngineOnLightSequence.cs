using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineOnLightSequence : MonoBehaviour {

	private Animator anim;

	// Use this for initialization
	private void Awake () {
		anim = GetComponent<Animator> ();
		anim.SetBool ("EngineOn", true);
	}
}
