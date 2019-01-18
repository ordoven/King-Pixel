using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour {
	private Rigidbody2D _movement;
	private SpriteRenderer _sprite;
	private Animator _characterAnimation;
	private AudioSource _audioSfx;
	public LayerMask GroundLayer;
	public LayerMask LadderLayer;
	[SerializeField] private Transform _groundCheck;
    [SerializeField] private float _walkSpeed = 2f;
	[SerializeField] private float _ladderSpeed = 2f;
	[FormerlySerializedAs("jumpHeight")] public float JumpHeight;
	private bool _grounded;
	public bool CanMove;
	public bool OnLadder;
	[Space] public AudioClip GroundHit;
	public GameObject JumpSoundObject;

	private void Start () {
        _characterAnimation = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _movement = GetComponent<Rigidbody2D>();
        _audioSfx = GetComponent<AudioSource>();
		OnLadder = false;
		_grounded = false;
    }

	public void FixedUpdate() {
		if (!CanMove) { // USED FOR DIALOG BOXES to stop players movement
			_movement.velocity = new Vector2(0f, 0f);
			_characterAnimation.SetFloat ("Speed", 0f);
			return;
		}

		if (OnLadder) return;
		IsOnLadder ();
		var input = Input.GetAxis ("Horizontal");
		_characterAnimation.SetFloat ("Speed", Mathf.Abs (input));
		_movement.velocity = new Vector2 (input * _walkSpeed * Time.deltaTime, _movement.velocity.y);
		if (input > 0 && _sprite.flipX) {
			_sprite.flipX = !_sprite.flipX;
		} else if (input < 0 && _sprite.flipX == false) {
			_sprite.flipX = !_sprite.flipX;
		}

	}

	public void Update() {
		_grounded = Physics2D.OverlapCircle (_groundCheck.position, 0.1f, GroundLayer);
		HandleMovementAnimation ();
		if (!CanMove) { // USED FOR DIALOG BOXES to stop players movement
			_movement.velocity = new Vector2(0f, 0f);
			_characterAnimation.SetFloat ("Speed", 0f);
			return;
		}
		if (!OnLadder) {
			if (!_grounded || !Input.GetKeyDown("space") && !Input.GetKeyDown("joystick button 3")) return; 
			_movement.AddForce (new Vector2 (0f, JumpHeight)); // Jump is registered
			Instantiate (JumpSoundObject, transform, true);
		} else {
			IsOffLadder ();
			_movement.velocity = Vector3.zero;
			if (_grounded) {
				OnLadder = false;
				_movement.isKinematic = false;
				_characterAnimation.SetFloat ("OnLadderSpeed", 0f);
				_characterAnimation.SetBool ("OnLadder", false);
			}

			var vertical = Input.GetAxis ("Vertical");
			_characterAnimation.SetFloat ("OnLadderSpeed", 0f);
			var horizontal = Input.GetAxis ("Horizontal");

			if (Math.Abs(vertical) > 0 || Math.Abs(horizontal) > 0) {
				_characterAnimation.SetFloat ("OnLadderSpeed", 1f);
			}

			if (vertical > 0f) {
				gameObject.transform.localPosition += new Vector3 (0f, _ladderSpeed*Time.deltaTime);
			}
			else if (vertical < 0f) {
				gameObject.transform.localPosition -= new Vector3 (0f, _ladderSpeed*Time.deltaTime);
			} else {
				gameObject.transform.localPosition += new Vector3 (0f, 0f);
			}

			if (horizontal > 0f) {
				gameObject.transform.localPosition += new Vector3 (_ladderSpeed*Time.deltaTime, 0f);
			} else if (horizontal < 0f) {
				gameObject.transform.localPosition -= new Vector3 (_ladderSpeed*Time.deltaTime, 0f);
			} else {
				gameObject.transform.localPosition += new Vector3 (0f, 0f);
			}
		}
	}

	private void HandleMovementAnimation()
	{
		if (_grounded) {
            if (_characterAnimation.GetFloat("State") > 0f && !_audioSfx.isPlaying) {
                _audioSfx.clip = GroundHit;
                _audioSfx.Play();
            }
			_characterAnimation.SetFloat ("State", 0f);
		}
		if (_movement.velocity.y > 0.4f)
			_characterAnimation.SetFloat ("State", 1f);
		else if (_movement.velocity.y < -0.4f)
			_characterAnimation.SetFloat ("State", 2f);

	}

	private void IsOnLadder()
	{
		var vertical = Input.GetAxis ("Vertical");
		if (!Physics2D.OverlapCircle(_groundCheck.position, 0.05f, LadderLayer) || !(vertical > 0f)) return;
		OnLadder = true;
		_movement.isKinematic = true;
		_characterAnimation.SetFloat ("Speed", 0f);
		_characterAnimation.SetFloat ("OnLadderSpeed", 1f);
		_characterAnimation.SetBool ("OnLadder", true);
		_movement.velocity = Vector3.zero;
	}

	private void IsOffLadder() 
	{
		if (Physics2D.OverlapCircle(_groundCheck.position, 0.05f, LadderLayer)) return;
		OnLadder = false;
		_movement.isKinematic = false;
		_characterAnimation.SetFloat ("OnLadderSpeed", 0f);
		_characterAnimation.SetBool ("OnLadder", false);
	}

	private static IEnumerator ChangeLevel()
	{
		var fadeTime = FindObjectOfType<Fading>().BeginFade(1);
		print(fadeTime);
		yield return new WaitForSeconds(fadeTime);
		FindObjectOfType<PortalS> ().GetComponent<PortalS> ().SwitchScene (SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void LevelContinue() // Triggered when player goes through the portal
	{ 
		StartCoroutine(ChangeLevel());
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Collectable")) {
			other.gameObject.GetComponent<Animator> ().Play ("PickUp");
		}
	}
}
