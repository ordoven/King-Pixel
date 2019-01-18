using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AssignMaterial : MonoBehaviour {

    private GameObject[] _bitch;
    [FormerlySerializedAs("preferredMat")] [SerializeField]
    private Material _preferredMat;

	void Awake ()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().material = _preferredMat;
        }
	}
}
