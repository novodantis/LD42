using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {
	public Text fundsCounter;
	public Text cashflowCounter;
	public Text happinessCounter;

	// sub-components
	public StarsystemInfoUI starsystemPanel;

	// referred objects
	public Economy economy;

	public void Start(){
		starsystemPanel.Close();
	}
	
	public void Update(){
		if (economy != null){
			fundsCounter.text = economy.funds.ToString();
			cashflowCounter.text = economy.income.ToString();
			happinessCounter.text = economy.happiness.ToString();
		}
	}
}
