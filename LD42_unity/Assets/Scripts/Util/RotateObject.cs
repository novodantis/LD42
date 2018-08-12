using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	public Vector3 rotationAxis;
	
	void Update () {
		transform.Rotate(rotationAxis * Time.deltaTime);
	}
}
