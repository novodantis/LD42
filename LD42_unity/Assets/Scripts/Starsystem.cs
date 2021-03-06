﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Starsystem : MonoBehaviour {

	public float gdp;					// income per population
	public float population;				// population count

	public GameObject star;				// visual object
	public GameObject destructionEffect;	// prefab spawned on destroying
	public GameObject remains;			// visual when destroyed
	public GridPointer marker;

	// sound
	public AudioClip destructionSfx;		// sound on destruction
	public bool destroyed;
	private AudioSource audioSource;
	private Economy localEconomy;
	private GridPointer myMarker;

	public void Awake(){
		audioSource = GetComponent<AudioSource>();
		localEconomy = FindObjectOfType<Economy>();
		if (marker != null){
			myMarker = Instantiate(marker, transform.position, Quaternion.identity);
			myMarker.transform.parent = transform.parent;
			myMarker.target = transform;
		} else {
			Debug.LogWarning("No marker defined for "+gameObject.name);
		}
		UpdateAppearance();
	}

	public bool Annihilate() {
		// destroy this system, return false if we can't
		if (localEconomy.Pay(GlobalSettings.demolishCost)){
			// destroy
			audioSource.PlayOneShot(destructionSfx);
			
			destroyed = true;

			if (destructionEffect != null)
				Instantiate(destructionEffect, transform.position, Quaternion.identity);

			UpdateAppearance();

			localEconomy.Process(false);				// update effects of this (but don't apply funds change)
			return true;
		} else {
			// can't afford it!
			return false;
		}
	}

	private void UpdateAppearance(){
		star.SetActive(!destroyed);
		remains.SetActive(destroyed);
	}
}
