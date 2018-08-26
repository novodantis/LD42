using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPointer : MonoBehaviour {

	public LineRenderer lineRenderer;
	public Transform target;
	public bool useGlobalY;

	public void Awake(){
		lineRenderer = GetComponent<LineRenderer>();
	}
	public void Update(){
		transform.position = new Vector3(
			target.position.x,
			useGlobalY ? 0 : transform.parent.position.y,
			target.position.z
		);
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, target.position);
	}
}
