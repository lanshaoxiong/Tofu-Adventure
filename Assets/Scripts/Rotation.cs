using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	public float rotationsPerMinute = 640f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo = Space.Self);
		// zAngle	Degrees to rotate around the Z axis.
		// Space.Self: the rotation is applied around the transform's local axes
		// Space.World: rotation is applied around the world x, y, z axes
		 
		transform.Rotate (0, 0, rotationsPerMinute * Time.deltaTime, Space.Self);
	}
}
