using UnityEngine;
using UnityEngine.Serialization;

public class Slime : MonoBehaviour {
	
	[FormerlySerializedAs("boostSize")] public int BoostSize;
	Rigidbody2D _player;
	AudioSource _aud;

	void Start()
	{
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ();
		_aud = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" || other.gameObject.layer != LayerMask.NameToLayer("Ground")) {
			_player.velocity = new Vector2 (_player.velocity.x, Mathf.Abs(_player.velocity.y) + BoostSize);
			_aud.Play ();
		}
	}
}
