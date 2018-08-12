using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Starsystem : Node {

	public float gdp;					// income per population
	public float population;				// population count

	public GameObject star;				// visual object
	public GameObject destructionEffect;	// prefab spawned on destroying
	public GameObject remains;			// visual when destroyed
	
	// sound
	public AudioClip destructionSfx;		// sound on destruction
	public bool destroyed;
	private AudioSource audioSource;

	public void Awake(){
		audioSource = GetComponent<AudioSource>();
		UpdateAppearance();
	}

	public void Annihilate() {
		// destroy this system
		audioSource.PlayOneShot(destructionSfx);
		
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
