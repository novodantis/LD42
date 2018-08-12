using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsystemInfoUI : MonoBehaviour {
	// show the details of a starsystem in UI

	public GameObject systemStats;		 // area where population, gdp etc is shown
	public GameObject buttons;			// all buttons
	public Text nameDisplay;
	public Text populationCounter;
	public Text gdpCounter;
	public Text incomeCounter;

	private Starsystem systemDisplayed;

	public void DisplayValuesFor(Starsystem _thisSystem){
		systemDisplayed = _thisSystem;

		if (systemDisplayed.destroyed){
			systemStats.SetActive(false);
			buttons.SetActive(false);
			nameDisplay.text = systemDisplayed.gameObject.name + " (destroyed)";
		} else {
			systemStats.SetActive(true);
			buttons.SetActive(true);
			nameDisplay.text = systemDisplayed.gameObject.name;

			populationCounter.text = systemDisplayed.population + " billion";
			gdpCounter.text = systemDisplayed.gdp + " mtons";
			incomeCounter.text = (systemDisplayed.population * systemDisplayed.gdp).ToString();
		}
		
		gameObject.SetActive(true);
	}

	public void Close(){
		PlayerControl _playerControl = FindObjectOfType<PlayerControl>();
		if (_playerControl != null){
			_playerControl.Deselect();
		} 
		gameObject.SetActive(false);
	}

	public void ActionRoute(){
		// start a Bypass from here
	}

	public void ActionDemolish(){
		// destroy this system!
		systemDisplayed.Annihilate();
		// update appearance
		DisplayValuesFor(systemDisplayed);
	}
}
