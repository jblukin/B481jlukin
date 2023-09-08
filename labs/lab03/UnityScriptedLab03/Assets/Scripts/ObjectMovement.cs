using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

	public float moveSpeed = 5;

	public float turnSpeed = 50;

	// Update is called once per frame
	void Update () {

	transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);

	}


}