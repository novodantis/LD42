using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]
public class TitleLogic : MonoBehaviour {
	public Image openingSlide;
	public Image titleSlide;
	public Button startButton;
	public Text infoText;

	public float minIntroTime = 2f;
	public AudioClip inputSound;
	public string levelName;
	private int progression;
	private AudioSource audioSource;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
		openingSlide.enabled = true;
		titleSlide.enabled = false;
		startButton.gameObject.SetActive(false);
		infoText.gameObject.SetActive(false);
	}
	public void Update(){
		if (Time.timeSinceLevelLoad > minIntroTime){
			if (Input.anyKeyDown && progression == 0){
				progression = 1;
				audioSource.PlayOneShot(inputSound);
			}
		}

		if (progression >= 1){
			// show title
			openingSlide.enabled = false;
			titleSlide.enabled = true;
			startButton.gameObject.SetActive(true);
			infoText.gameObject.SetActive(true);
		}
	}
	
	public void StartGame(){
		SceneManager.LoadScene(levelName);
	}
}
