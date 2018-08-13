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

	public Starsystem systemDisplayed;

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

			populationCounter.text = (Mathf.Round(systemDisplayed.population * 10f) / 10f) + " billion";
			gdpCounter.text = (Mathf.Round(systemDisplayed.gdp * 10f) / 10f) + " mtons";
			incomeCounter.text = (Mathf.Round((systemDisplayed.population * systemDisplayed.gdp) * 100f) / 100f).ToString();
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
		// start/continue/end a Bypass from here
	}

	public void ActionDemolish(){
		// destroy this system!
		systemDisplayed.Annihilate();
		// update appearance
		DisplayValuesFor(systemDisplayed);
	}
}
