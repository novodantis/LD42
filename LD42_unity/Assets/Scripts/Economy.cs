using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Economy : MonoBehaviour {
	// manages the galaxy economy
	public int turnCounter;
	public float income;				// amount being made per tick
	public float funds;					// amount stored
	public float happiness;				// population joy
	public float devestationUnhappiness = 2f;	// multiplier for how angry destruction makes the people

	public Starsystem[] allSystems;
	public Bypass[] allBypasses;
	public MainUI userInterface;
	private AudioSource audioSource;
	public AudioClip nextTurnSound;

	public void Start(){
		audioSource = GetComponent<AudioSource>();
		allSystems = FindObjectsOfType<Starsystem>();		// get all star systems

		ProcessTick();
	}

	public void ProcessTick(){
		if (nextTurnSound != null)
			audioSource.PlayOneShot(nextTurnSound);
		Process(true);
		turnCounter ++;
	}

	public void Process(bool _applyIncome){
		// calculate income
		income = 0;
		happiness = 0;
		foreach(Starsystem sys in allSystems){
			if (!sys.destroyed){
				income += sys.gdp * sys.population;
				happiness += sys.population;
			} else {
				happiness -= sys.population * devestationUnhappiness;
			}
		}

		// Bypasses
		allBypasses = FindObjectsOfType<Bypass>();

		foreach(Bypass b in allBypasses){
			if (b.completed){
				// check if destroyed
				if (b.nodes[0].destroyed){
					b.Remove(false);
				} else if (b.GetLastNode().destroyed){
					b.Remove(false);
				} else {
					income += b.worth;
				}
			}
				
		}

		if (_applyIncome)
			funds += income;
	}

	public bool Pay(int _cost){
		// returns false if cannot afford
		if (funds >= _cost){
			funds -= _cost;
			return true;
		} else {
			Debug.LogWarning("Can't afford it!");
			return false;
		}
	}
}
