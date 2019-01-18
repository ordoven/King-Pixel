using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class SnakeMovement : MonoBehaviour {
	[FormerlySerializedAs("snakeSpeed")] public float SnakeSpeed;
	Rigidbody2D _somethin;
	Vector3 _currentPosition;
	private bool _left;
	// Use this for initialization
	void Start () {
		_somethin = GetComponent<Rigidbody2D> ();
		_currentPosition = _somethin.transform.localPosition;
		_left = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (_left) {
			_currentPosition.x -= SnakeSpeed;
		} else {
			_currentPosition.x += SnakeSpeed;
		}
		_somethin.transform.localPosition = _currentPosition;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name == "Bound") { // GameObjects name is "bound"
			_left = !_left;
		}
	}

}
