using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class Destroy : MonoBehaviour {
	[FormerlySerializedAs("delay")] public float Delay;

	void Start(){
		Destroy (gameObject, Delay);
	}

}
