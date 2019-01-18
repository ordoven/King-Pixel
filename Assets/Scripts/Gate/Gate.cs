using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Gate : MonoBehaviour {
	[FormerlySerializedAs("keyObject")] public GameObject KeyObject;
	public bool PlayerEntered { get; set;}

	void OnTriggerStay2D(Collider2D other){
		if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E) && KeyObject == null) {
			Destroy (gameObject);
		}

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			PlayerEntered = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player") {
			PlayerEntered = false;
		}
	}
}