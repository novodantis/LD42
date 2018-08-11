﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : MonoBehaviour {
	// manages the galaxy economy
	public float income;				// amount being made per tick
	public Starsystem[] allSystems;

	public void Start(){
		allSystems = FindObjectsOfType<Starsystem>();		// get all star systems

		UpdateIncome();
	}

	private void UpdateIncome(){
		// calculate income
		income = 0;
		foreach(Starsystem sys in allSystems){
			if (!sys.destroyed){
				income += sys.gdp * sys.population;
			}
		}
	}
}