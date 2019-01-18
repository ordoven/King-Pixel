using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BackgroundMusicManagerBehaviour : MonoBehaviour
{
    [FormerlySerializedAs("mainMenuMusic")] public AudioClip[] MainMenuMusic;
    [FormerlySerializedAs("gamePlayMusic")] public AudioClip[] GamePlayMusic;
    [FormerlySerializedAs("gameOverMusic")] public AudioClip[] GameOverMusic;
    [FormerlySerializedAs("music_VOLUME")]
    [Space]
    [Range(0f, 1f)]
    public float MusicVolume = 1f;
    [FormerlySerializedAs("game_VOLUME")] [Range(0f, 1f)]
    public float GameVolume = 1f;

    private AudioSource _soundController;
    private string _currentSceneName;
    private bool _gameOver;
	private AudioClip _previousMusic;

    void Start() {
        _gameOver = false;
        _soundController = GetComponent<AudioSource>();
        _currentSceneName = SceneManager.GetActiveScene().name;
    }

    void Update() {
		_soundController.volume = MusicVolume;

		if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthHandle>().GameOver) {
                _gameOver = false;
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthHandle>().GameOver && !_gameOver)
            {
                _soundController.clip = null;
                _gameOver = true;
            }
        }
			
        if (_currentSceneName != SceneManager.GetActiveScene().name)
        { 
            _soundController.Stop();
            _soundController.clip = null;
            _currentSceneName = SceneManager.GetActiveScene().name;
        }
        if (_soundController.clip == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") == null) // Main menu
            {
                _soundController.clip = MainMenuMusic[Random.Range(0, MainMenuMusic.Length)];
				while (_soundController.clip == _previousMusic) {
					_soundController.clip = MainMenuMusic[Random.Range(0, MainMenuMusic.Length)];
				}
            }
            else if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthHandle>().GameOver)
            {
                _soundController.clip = GameOverMusic[Random.Range(0, GameOverMusic.Length)];
				while (_soundController.clip == _previousMusic) {
					_soundController.clip = GameOverMusic[Random.Range(0, GameOverMusic.Length)];
				}
            } // Normal game screen
			else if (FindObjectOfType<CameraMovement>().GetComponent<CameraMovement>().SmoothFollow)
            {
                _soundController.clip = GamePlayMusic[Random.Range(0, GamePlayMusic.Length)];
				while (_soundController.clip == _previousMusic) {
					_soundController.clip = _soundController.clip = GamePlayMusic[Random.Range(0, GamePlayMusic.Length)];
				}
            }
			_previousMusic = _soundController.clip;
            _soundController.Play();
        }
    }

	public void ChangeMusic()
	{
		if (_soundController.isPlaying) {
			_soundController.clip = GamePlayMusic[Random.Range(0, GamePlayMusic.Length)];
			while (_soundController.clip == _previousMusic) {
				_soundController.clip = GamePlayMusic[Random.Range(0, GamePlayMusic.Length)];
			}
			_previousMusic = _soundController.clip;
			_soundController.Play();
		}
	}
}
