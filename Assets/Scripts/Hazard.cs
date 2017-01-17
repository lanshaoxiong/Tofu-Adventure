using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	public GameObject playerDeathPrefab;
	public AudioClip deathClip;
	public Sprite hitSprite;
	private SpriteRenderer spriteRenderer;


	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.transform.tag == "Player") {
			var audioSource = GetComponent<AudioSource> ();
			if (audioSource != null && deathClip != null) {
				audioSource.PlayOneShot (deathClip);
			}
			// public static Object Instantiate(Object original, Vector3 position, Quaternion rotation);
			// "no rotation" - the object is perfectly aligned with the world or parent axes.
			Instantiate(playerDeathPrefab, coll.contacts[0].point, Quaternion.identity);
			spriteRenderer.sprite = hitSprite; // the saw blade with soy blood covered 
			Destroy (coll.gameObject);
			GameManager.instance.RestartLevel (1.25f);// restart the level after a 1.25 second delay
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
