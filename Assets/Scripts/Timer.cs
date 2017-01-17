using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private Text timerText;

	// Use this for initialization
	void Awake () {
		timerText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		timerText.text = System.Math.Round ((decimal)Time.timeSinceLevelLoad, 2).ToString (); // round two decimal place	
	}
}
