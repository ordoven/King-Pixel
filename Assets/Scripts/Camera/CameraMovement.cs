using UnityEngine;
using System.Collections;
using UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour {
	
	[FormerlySerializedAs("cameraSpeed")] public float CameraSpeed;
	[FormerlySerializedAs("offset")] public float Offset;
	[FormerlySerializedAs("smoothFollow")] public bool SmoothFollow = false;
	private float _shakeDur = 0f, _shakeAmount = 0f;
	private GameObject _mainCharacter;
	[FormerlySerializedAs("sceneStartLookDelay")] public float SceneStartLookDelay = 5f;
	[FormerlySerializedAs("timer")] public Text Timer;
	[FormerlySerializedAs("levelSelect")] public GameObject LevelSelect;

    private void Awake() {
		foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
			obj.GetComponent<EnemyController> ().CanMove = false;
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>().enabled = false;
		Timer.gameObject.SetActive (false);
    }

	private void Start()
	{
		//FindObjectOfType<Fading>().BeginFade(-1);
	}

	private void FixedUpdate() {
        if (_mainCharacter == null) { // Fail-safe
            try { _mainCharacter = GameObject.FindGameObjectWithTag("Player"); } 
            catch { return; }
        } 
		else {
			if (SmoothFollow)
				FollowPlayer ();
			ShakeHandle ();

			if (LevelSelect == null && Timer != null)
				Timer.gameObject.SetActive (true);
			
			if (SceneStartLookDelay <= 0f && transform.position.z != -10f) { // if timer reaches 0
				if (Timer != null) { // Destroy the timer UI object
					Destroy (Timer.gameObject, .2f);
					Timer = null;
				}
				if (!SmoothFollow) { // Enables game entity movement (once)
					FindObjectOfType<BackgroundMusicManagerBehaviour> ().GetComponent<BackgroundMusicManagerBehaviour> ().ChangeMusic ();
					GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ().enabled = true;
					foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
						obj.GetComponent<EnemyController> ().CanMove = true;
				}
				SmoothFollow = true; // Enable camera to follow player
				//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x,
				//	transform.position.y, -10f), 0.2f); // Move towards z = -10
			} 
			else if (Timer != null && Timer.gameObject.activeSelf) { // Count down
				SceneStartLookDelay -= Time.deltaTime;
				Timer.text = Mathf.Ceil (SceneStartLookDelay).ToString ();
			}

        }
	}

	private void FollowPlayer() {
		var newPos = transform.position;
		newPos.x = _mainCharacter.transform.position.x - Offset;
		newPos.y = _mainCharacter.transform.position.y - Offset;
		transform.position = Vector3.Lerp (transform.position, newPos, CameraSpeed * Time.deltaTime);
		transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x,
			transform.position.y, -10f), 0.2f);
	}

	private void ShakeHandle() {
		if (!(_shakeDur > 0f)) return;
		var shake = Random.insideUnitCircle * _shakeAmount;
		transform.position += new Vector3 (shake.x, shake.y, 0f);
		_shakeDur -= Time.deltaTime;
	}

	public void CameraShake(float amount, float duration) {
		_shakeAmount = amount;
		_shakeDur = duration;
	}
		
	#region GameOver functions
	public void GotoMainMenu(int index) {
		StartCoroutine(ChangeLevel(index));
	}

	public void Retry() {
		//FindObjectOfType<BackgroundMusicManagerBehaviour> ().GetComponent<BackgroundMusicManagerBehaviour> ().ChangeMusic ();
		StartCoroutine (ChangeLevel (SceneManager.GetActiveScene ().buildIndex));
	}

	private static IEnumerator ChangeLevel(int levelIndex) {
		yield return new WaitForSeconds(GameObject.Find("Managers").GetComponent<Fading>().BeginFade(1));
		SceneManager.LoadScene(levelIndex);
	}
	#endregion
}
