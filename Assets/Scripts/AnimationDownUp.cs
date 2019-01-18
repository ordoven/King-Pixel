using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationDownUp : MonoBehaviour {
	private Vector3 _startingPos;
	bool _decending;
	[FormerlySerializedAs("travelAmount")] public float TravelAmount = 1f;
	[FormerlySerializedAs("travelPerFrame")] public float TravelPerFrame = 0.1f;

	void Start () {
		_startingPos = transform.position;	
		_decending = false;
	}

	void FixedUpdate () {
		if (!_decending) {
			transform.position = Vector3.MoveTowards (transform.position, _startingPos + new Vector3 (0f, TravelAmount, 0f), TravelPerFrame);
			if (transform.position == _startingPos + new Vector3 (0f, TravelAmount, 0f))
				_decending = true;
		}
		else {
			transform.position = Vector3.MoveTowards (transform.position, _startingPos - new Vector3 (0f, TravelAmount, 0f), TravelPerFrame);
			if (transform.position == _startingPos - new Vector3 (0f, TravelAmount, 0f))
				_decending = false;
		}
			
	}

	public void DestroyObject(float delay){
		Destroy (gameObject, delay);
	}
}
