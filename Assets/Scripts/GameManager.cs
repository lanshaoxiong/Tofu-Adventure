using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;  // the singleton instance script: only have a single instance

	// to reload Game scene

	//public Coroutine StartCoroutine(IEnumerator routine); 
	// StartCoroutine:  invoke a coroutine and continue executing the function in parallel.
	// WaitForSeconds: Suspends the coroutine execution for the given amount of seconds using scaled time.
	// WaitForSeconds can only be used with a yield statement in coroutines.



	public void RestartLevel(float delay){
		StartCoroutine (RestartLevelDelay(delay));
	}
	private IEnumerator RestartLevelDelay(float delay){
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene ("Game");
	}

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		// public static void DontDestroyOnLoad(Object target)
		// Makes the object target not be destroyed automatically when loading a new scene.
		// In order to preserve an object during level loading call DontDestroyOnLoad on it
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
