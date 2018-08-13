using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Elenesski.Camera.Utilities;

public class PlayerControl : MonoBehaviour {
	// overall management of player
	public Camera cameraObject;
	public GenericMoveCamera standardCam;
	public MouseOrbitImproved orbitCam;

	// other references
	public MainUI userInterface;

	private bool systemSelected;		// true if target planet currently selected
	private Starsystem selectedTarget;		// target Node-type object clicked on

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
	}

	public void SelectStarsystem(Starsystem _selected){
		selectedTarget = _selected;
		systemSelected = true;

		standardCam.enabled = false;
		// TO DO: Tween the cam nicely to orbit mode; can make its target assigned to an E.G.O, which lerps toward system targets
		orbitCam.target = selectedTarget.transform;
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
