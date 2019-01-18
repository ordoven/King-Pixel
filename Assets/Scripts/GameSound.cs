using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSound : MonoBehaviour {
	private BackgroundMusicManagerBehaviour _musicManager;
	private AudioSource _audioSrc;

	private void Start () {
		_musicManager = FindObjectOfType<BackgroundMusicManagerBehaviour> ().GetComponent<BackgroundMusicManagerBehaviour> ();
		_audioSrc = GetComponent<AudioSource> ();
		_audioSrc.spatialBlend = 0f;
		_audioSrc.minDistance = 0.8f;
		_audioSrc.maxDistance = 1f;
	}

	private void Update(){
		_audioSrc.volume = _musicManager.GameVolume;
	}
}
