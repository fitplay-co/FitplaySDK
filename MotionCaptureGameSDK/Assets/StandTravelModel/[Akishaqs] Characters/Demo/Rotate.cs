using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2 : MonoBehaviour {

	public int speed = 5;

	// Use this for initialization
	void Update () 
	{
		transform.Rotate (0,0, speed * Time.deltaTime);
	}
}
