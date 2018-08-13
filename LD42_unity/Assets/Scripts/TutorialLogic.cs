using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class TutorialLogic : MonoBehaviour {

	// refs
	public Flowchart tutorialScript;
	public Starsystem clickOnTut1;
	public Starsystem clickOnTut2;
	public MainUI userInterface;
	private Economy localEconomy;

	public void Awake(){
		localEconomy = FindObjectOfType<Economy>();
	}
	public void Update(){
		if (userInterface.starsystemPanel.systemDisplayed == clickOnTut1){
			tutorialScript.SetBooleanVariable("selectedPlanet", true);
		}
		if (clickOnTut1.destroyed){
			tutorialScript.SetBooleanVariable("earthDestroyed", true);
		}
		if (userInterface.starsystemPanel.systemDisplayed != clickOnTut1){
			tutorialScript.SetBooleanVariable("selectedEridani", true);
		}
		if (localEconomy.turnCounter == 12){
			tutorialScript.SetBooleanVariable("summary", true);
			tutorialScript.SetIntegerVariable("score", Mathf.RoundToInt(localEconomy.funds));
		}
	} 
}
