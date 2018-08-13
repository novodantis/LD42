using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TutorialLogic : MonoBehaviour {

	// refs
	public Flowchart tutorialScript;
	public Starsystem clickOnTut1;
	public MainUI userInterface;

	public void Update(){
		if (userInterface.starsystemPanel.systemDisplayed == clickOnTut1){
			tutorialScript.SetBooleanVariable("selectedPlanet", true);
		}
	} 
}
