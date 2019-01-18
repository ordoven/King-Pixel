using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class MainMenuManager : MonoBehaviour {
	[FormerlySerializedAs("startScreen")] public GameObject StartScreen;
	[FormerlySerializedAs("options")] public GameObject Options;
	[FormerlySerializedAs("controls")] public GameObject Controls;
	[FormerlySerializedAs("_eventsystem")] public EventSystem Eventsystem;
	float _delay = 4f;
	bool _started = false;

	void Start () {
		if (!_started && Time.realtimeSinceStartup < 15f) {
			StartCoroutine (Before ());
			_started = true;
			SwitchToControlScreen ();
		} else {
			SwitchToStartScreen ();
		}
	}

	void Update(){
		if (Eventsystem.currentSelectedGameObject == null) {
			Eventsystem.SetSelectedGameObject(Eventsystem.firstSelectedGameObject);
		}		
	}

	IEnumerator Before(){
		yield return new WaitForSeconds (_delay);
		SwitchToStartScreen ();
	}

	public void SwitchToOptionScreen(){
		Controls.SetActive (false);
		StartScreen.SetActive (false);
		Options.SetActive (true);
		Eventsystem.SetSelectedGameObject (Options.transform.GetChild (0).gameObject);
	}

	public void SwitchToStartScreen(){
		Controls.SetActive (false);
		StartScreen.SetActive (true);
		for (var i = 0; i < StartScreen.transform.childCount; i++)
		{
			StartScreen.transform.GetChild(i).gameObject.SetActive(true);
		}

		Options.SetActive (false);
		Eventsystem.SetSelectedGameObject (Eventsystem.firstSelectedGameObject);
	}

	public void SwitchToControlScreen(){
		Controls.SetActive (true);
		StartScreen.SetActive (false);
		Options.SetActive (false);
		if (_started) {
			Controls.transform.GetChild (0).gameObject.SetActive (false);
			_started = false;
		} else {
			Controls.transform.GetChild (0).gameObject.SetActive (false);
			StartCoroutine (ButtonDelay (2f));
		}
	}

	IEnumerator ButtonDelay(float delay){
		yield return new WaitForSeconds (delay);
		Controls.transform.GetChild (0).gameObject.SetActive (true);
		Eventsystem.SetSelectedGameObject (Controls.transform.GetChild (0).gameObject);
	}
}
