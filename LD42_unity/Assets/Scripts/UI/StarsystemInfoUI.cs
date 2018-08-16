using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsystemInfoUI : MonoBehaviour {
	// show the details of a starsystem in UI

	public GameObject systemStats;		 // area where population, gdp etc is shown
	public GameObject systemButtons;			// buttons that are hidden when system destroyed

	public Text nameDisplay;
	public Text populationCounter;
	public Text gdpCounter;
	public Text incomeCounter;
	public Image targetReticle;
	public Color normalColour = Color.green;
	public Color routingColour = Color.blue;
	public Color demolishingColour = Color.red;

	// INSTANCE
	public Starsystem systemDisplayed;

	// ROUTING
	public GameObject routingButtons;	// buttons for Routing mode
	public GameObject startRouteButton;	// button for going into Routing
	public bool routingMode;			// building a Bypass
	public Bypass bypassPrefab;			// prefab to instantiate
	public Bypass newBypass;			// ref to currently building Bypass
	public Text bypassCostDisplay;		// cost to build Bypass
	public GameObject bypassProfitProjection;	// showing possible profits
	public Text bypassProfitDisplay;	// shows profit value

	// DEMOLISHING
	public GameObject demolishButtons;	// buttons to ok or cancel demolish
	public bool demolishingMode;		// ready to destroy!
	public Text demolishCostDisplay;	// cost to destroy planet

	public void Awake(){
		SetMode(0);
	}

	public void DisplayValuesFor(Starsystem _thisSystem){
		systemDisplayed = _thisSystem;
		bypassCostDisplay.text = "";

		if (systemDisplayed.destroyed){
			systemStats.SetActive(false);
			systemButtons.SetActive(false);
			bypassProfitProjection.SetActive(false);
			nameDisplay.text = systemDisplayed.gameObject.name + " (destroyed)";

			if (!routingMode){
				startRouteButton.SetActive(false);
			} else {
				startRouteButton.SetActive(true);
				if (newBypass.CheckRange(systemDisplayed.transform.position)){
					bypassCostDisplay.text = "CONNECT";
				} else {
					bypassCostDisplay.text = "OUT OF RANGE";
				}
			}
		} else {
			systemStats.SetActive(true);
			systemButtons.SetActive(true);
			startRouteButton.SetActive(true);
			bypassProfitProjection.SetActive(false);
			nameDisplay.text = systemDisplayed.gameObject.name;

			populationCounter.text = (Mathf.Round(systemDisplayed.population * 10f) / 10f) + " billion";
			gdpCounter.text = (Mathf.Round(systemDisplayed.gdp * 10f) / 10f) + " mtons";
			incomeCounter.text = (Mathf.Round((systemDisplayed.population * systemDisplayed.gdp) * 100f) / 100f).ToString();

			if (newBypass != null && routingMode){
				if (newBypass.nodes.Count > 0 && newBypass.GetLastNode() != systemDisplayed){
					if (newBypass.CheckUniqueNode(systemDisplayed)){
						if (newBypass.CheckUnique(systemDisplayed)){
							if (newBypass.CheckRange(systemDisplayed.transform.position)){
								// could complete here
								bypassCostDisplay.text = "-" + newBypass.GetCost(true);
								bypassProfitProjection.SetActive(true);
								bypassProfitDisplay.text = "+"+newBypass.ProjectProfit(systemDisplayed);
							} else {
								bypassCostDisplay.text = "OUT OF RANGE";
							}
						} else {
							bypassCostDisplay.text = "ALREADY MADE";
						}
					} else {
						bypassCostDisplay.text = "CANNOT RE-TREAD";
					}
					
				} else {
					bypassCostDisplay.text = "BUILDING...";
				}
			}
				
		}

		
		
		gameObject.SetActive(true);
	}

	public void Open(){
		//refresh values
		// if there's a bypass under construction, continue it
		if (newBypass != null)
			SetMode(2);
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
		if (routingMode) {
			// continue route logic
			newBypass.Route(systemDisplayed);
			if (newBypass.completed){
				// all done!
				newBypass = null;
				SetMode(0);
			}
		} else {
			// start route logic
			if (newBypass == null){
				newBypass = Instantiate(bypassPrefab, systemDisplayed.transform.position, Quaternion.identity);
				newBypass.Begin(systemDisplayed);
			} else {
				// continue old one
			}
			SetMode(2);
			
		}
	}

	public void ActionRouteCancel(){
		// stop building this Bypass
		routingMode = false;
		Destroy(newBypass.gameObject);
		SetMode(0);
	}

	public void ActionDemolishReady(){
		// ready to destroy
		if (demolishingMode){
			// pressing again cancels
			SetMode(0);
		} else {
			SetMode(1);
		}
		
	}

	public void ActionDemolishCancel(){
		// quit demolish mode
		SetMode(0);
	}

	public void ActionDemolish(){
		// destroy this system!
		if (systemDisplayed.Annihilate()){
			// update appearance
			DisplayValuesFor(systemDisplayed);
			SetMode(0);
		}
	}

	private void SetMode(int _mode){
		switch(_mode){
			case 0:
				// normal
				routingMode = false;
				demolishingMode = false;
				targetReticle.color = normalColour;
				demolishCostDisplay.text = "";
				bypassCostDisplay.text = "";
				bypassProfitProjection.SetActive(false);
				break;
			case 1:
				// demolish
				routingMode = false;
				demolishingMode = true;
				targetReticle.color = demolishingColour;
				demolishCostDisplay.text = "-" + GlobalSettings.demolishCost;
				bypassCostDisplay.text = "";
				bypassProfitProjection.SetActive(false);
				break;
			case 2:
				// routing
				routingMode = true;
				demolishingMode = false;
				targetReticle.color = routingColour;
				demolishCostDisplay.text = "";
				break;
		}

		demolishButtons.SetActive(demolishingMode);
		routingButtons.SetActive(routingMode);

		DisplayValuesFor(systemDisplayed);
	}
}
