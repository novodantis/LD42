using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starsystem : Node {

	public float gdp;					// income per population
	public float population;				// population count

	public GameObject star;				// visual object
	public GameObject destructionEffect;	// prefab spawned on destroying
	public GameObject remains;			// visual when destroyed
	
	public bool destroyed;

	public void Awake(){
		UpdateAppearance();
	}

	public void Annihilate() {
		// destroy this system
		destroyed = true;

		if (destructionEffect != null)
			Instantiate(destructionEffect, transform.position, Quaternion.identity);

		UpdateAppearance();
	}

	private void UpdateAppearance(){
		star.SetActive(!destroyed);
		remains.SetActive(destroyed);
	}
}
