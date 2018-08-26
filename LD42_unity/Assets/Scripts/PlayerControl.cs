using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Elenesski.Camera.Utilities;

public class PlayerControl : MonoBehaviour {
	// overall management of player
	public Camera cameraObject;
	public GenericMoveCamera standardCam;
	public MouseOrbitImproved orbitCam;
	public float camLerpSpeed = 1f;
	public AnimationCurve tweenCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

	// other references
	public MainUI userInterface;

	private bool systemSelected;		// true if target planet currently selected
	private Starsystem selectedTarget;		// target Node-type object clicked on
	private GameObject viewLocus;			// locus of camera
	private Vector3 viewTarget;				// where locus is headed to
	private Vector3 viewStart;				// where locus started
	private float viewProgress;				// progression to target of locus
	public void Awake(){
		viewLocus = new GameObject();
		ResetCamTarget();
	}
	public void Update(){
		if (Input.GetMouseButtonDown(0)){
			//Debug.Log("Selecting...");

			RaycastHit newHit = new RaycastHit();

			bool hasHit = Physics.Raycast(cameraObject.ScreenPointToRay(Input.mousePosition), out newHit);
			if (hasHit){
				Debug.Log("Hit object: "+newHit.collider.gameObject.name);
				Starsystem _toSelect = newHit.collider.gameObject.GetComponent<Starsystem>();
				if (_toSelect != null){
					Debug.Log("Object selected has Starsystem component!");
					SelectStarsystem(_toSelect);
				} else {
					Debug.Log("Hit something else");
				}
			} else {
			}
		}

		if (viewProgress < 1){
			// lerp view
			viewProgress += Time.deltaTime * camLerpSpeed;
			viewLocus.transform.position = Vector3.Lerp(viewStart, viewTarget, tweenCurve.Evaluate(viewProgress));
		}
	}

	public void ResetCamTarget(){
		orbitCam.target = viewLocus.transform;
		viewLocus.transform.position = transform.position;
		viewLocus.transform.rotation = transform.rotation;
		viewLocus.transform.Translate(0,0,orbitCam.distance, Space.Self);				// place target in front of us
		orbitCam.ResetView(transform.eulerAngles.y, transform.eulerAngles.x);
	}

	public void SelectStarsystem(Starsystem _selected){
		selectedTarget = _selected;
		systemSelected = true;

		if (standardCam.enabled){
			// not in planet view
			ResetCamTarget();
			standardCam.enabled = false;
			Debug.Log("Not yet selected a planet");
		} else {
			// currently viewing another planet
			Debug.Log("Selecting another planet");
		}
		viewTarget = selectedTarget.transform.position;
		viewStart = orbitCam.target.position;
		viewProgress = 0;
		
		orbitCam.enabled = true;

		userInterface.starsystemPanel.DisplayValuesFor(selectedTarget);
	}

	public void Deselect() {
		systemSelected = false;

		standardCam.enabled = true;
		orbitCam.enabled = false;

		//userInterface.starsystemPanel.Close();
	}
}
