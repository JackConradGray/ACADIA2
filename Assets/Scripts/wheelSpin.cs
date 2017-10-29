using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour {

	public float speed = 100f;


	void Update ()
	{
		transform.Rotate(Vector2.up, speed * Time.deltaTime);
	}
}