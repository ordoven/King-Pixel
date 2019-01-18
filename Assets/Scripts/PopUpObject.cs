using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpObject : MonoBehaviour {

	[FormerlySerializedAs("stringLine")] public string StringLine;
	[FormerlySerializedAs("delay")] public float Delay;
	[FormerlySerializedAs("screenActive")] public bool ScreenActive;
	TextMesh _txt;
	bool _typing;

	void Start () {
		ScreenActive = false;
		_typing = false;
		_txt = GetComponent<TextMesh> ();
	}
	
	void Update () {
		ScreenActive = transform.parent.gameObject.GetComponent<Gate> ().PlayerEntered;
		if (!ScreenActive) {
			_txt.text = "";
			_typing = false;
		} else if (!_typing) {
			StartCoroutine (TextScroll (StringLine));
		}
	}

	IEnumerator TextScroll(string input){
		_typing = true;
		string temp = "";
		for (int i = 0; i < input.Length && ScreenActive; i++) {
			temp += input [i];
			_txt.text = temp;
			Debug.Log ("doing");
			yield return new WaitForSeconds (Delay);
		}
		_txt.text = input;
	}
}
