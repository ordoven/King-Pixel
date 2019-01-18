using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DartTrap : MonoBehaviour {
	[FormerlySerializedAs("delay")] public float Delay;
	[FormerlySerializedAs("dart")] public GameObject Dart;
	float _tim;
	Animator _animation;
	SpriteRenderer _sprite;
	AudioSource _audio;
	bool _preparing = false;

	void Start ()
	{
		_tim = 0f;
		_animation = GetComponent<Animator> ();
		_sprite = GetComponent<SpriteRenderer> ();
		_audio = GetComponent<AudioSource> ();
	}

	void FixedUpdate () 
	{
		if (Delay <= _tim && !_preparing) {
			FireShot ();
		} else
			_tim += Time.deltaTime;
	}

	void FireShot()
	{
		_preparing = true;
		_animation.Play ("Prep");
	}

	public void Fire(){
		_animation.Play ("FireShot");
		_audio.Play ();
		var obj = Instantiate (Dart, transform.position, Quaternion.identity);
		if (!_sprite.flipX) {
			obj.transform.position += new Vector3 (.8f, 0f, 0f);
			obj.GetComponent<Weapon> ().Initialize (Vector2.right);
		} else {
			obj.transform.position -= new Vector3 (.8f, 0f, 0f);
			obj.GetComponent<Weapon> ().Initialize (Vector2.left);
		}
		obj.GetComponent<SpriteRenderer> ().flipX = _sprite.flipX;
		_preparing = false;
		_tim = 0;
	}
}
