using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour {

	BackgroundMusicManagerBehaviour _musicManager;
	[FormerlySerializedAs("musicSlider")] public Slider MusicSlider;
	[FormerlySerializedAs("gameSlider")] public Slider GameSlider;

	void Start () {
		_musicManager = FindObjectOfType<BackgroundMusicManagerBehaviour> ().GetComponent<BackgroundMusicManagerBehaviour> ();
		UpdateOptionsInformation ();
	}

	public void OnValueChangeMusicVolume(){
		_musicManager.MusicVolume = MusicSlider.value;
	}

	public void OnValueChangeGameVolume(){
		_musicManager.GameVolume = GameSlider.value;
	}

	void UpdateOptionsInformation(){
		MusicSlider.value = _musicManager.MusicVolume;
		GameSlider.value = _musicManager.GameVolume;
	}
}
