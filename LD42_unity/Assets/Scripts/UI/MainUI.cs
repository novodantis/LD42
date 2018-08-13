using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {
	public Text fundsCounter;
	public Text cashflowCounter;
	public Text happinessCounter;
	public Text turnCounter;

	// sub-components
	public StarsystemInfoUI starsystemPanel;

	// referred objects
	public Economy economy;

	public void Start(){
		starsystemPanel.Close();
	}
	
	public void Update(){
		if (economy != null){
			fundsCounter.text = Mathf.Round(economy.funds).ToString();
			cashflowCounter.text = economy.income.ToString();
			happinessCounter.text = economy.happiness.ToString();

			turnCounter.text = economy.turnCounter.ToString();
		}
	}
}
