using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {
	[FormerlySerializedAs("textField")] public Text TextField;
	Slider _sliderValue;

	void Start () {
		_sliderValue = GetComponent<Slider> ();
	}

	void Update () {
		TextField.text = Mathf.RoundToInt(_sliderValue.value * 100).ToString() + " %";
	}
}
