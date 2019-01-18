using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PortalS : MonoBehaviour {

    private bool _playerEnter, _succ;
    AudioSource _sfxAudio;
    private float _volume;
	[FormerlySerializedAs("lastLevel")] public bool LastLevel = false;
	bool _collected = false;

	void Start ()
    {
        _sfxAudio = GetComponent<AudioSource>();
		_volume = FindObjectOfType<BackgroundMusicManagerBehaviour> ().GetComponent<BackgroundMusicManagerBehaviour> ().GameVolume;
        _playerEnter = false;
		_succ = false;
    }
	
	void Update ()
    {
		if (GameObject.FindGameObjectsWithTag ("Collectable").Length <= 0)
			_collected = true;
		if (_succ) {
			GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.MoveTowards(
				GameObject.FindGameObjectWithTag("Player").transform.position, transform.position, 0.05f);
		}
        if (_playerEnter && !_sfxAudio.isPlaying) {
            _sfxAudio.Play();
        } else if (_playerEnter) {
            _sfxAudio.volume = Mathf.MoveTowards(_sfxAudio.volume, _volume, 0.08f);
        } else if (!_playerEnter && _sfxAudio.isPlaying) {
            _sfxAudio.volume = Mathf.MoveTowards(_sfxAudio.volume, 0f, 0.08f);
        }

		if (_playerEnter && Input.GetKeyDown (KeyCode.E) && _collected) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<Animator> ().Play ("PortalEnterKing");
			_succ = true;
		} else if (_playerEnter && Input.GetKeyDown (KeyCode.E) && !_collected) {
			FindObjectOfType<Camera> ().GetComponent<CameraMovement> ().CameraShake (.25f, 1f);
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player") {
            _playerEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player") {
            _playerEnter = false;
        }
    }

	public void SwitchScene(int index){
		if (LastLevel)
			SceneManager.LoadScene (0);
		else
			SceneManager.LoadScene (index);
	}
}
