using UnityEngine;
using UnityEngine.Serialization;

public class TitleAnimation : MonoBehaviour {

	[FormerlySerializedAs("travelDistance")] public float TravelDistance;
	[FormerlySerializedAs("speed")] public float Speed = 20f;
	private bool _once;
	RectTransform _rTrans;
	Animator _anim;

	void Start () {
		_rTrans = GetComponent<RectTransform>();
		_anim = GetComponent<Animator>();
		_once = true;
		DeactivateChildren();
	}
	
	void Update () {
		var l = _rTrans.localPosition;
		l = Vector3.MoveTowards(l, new Vector3(l.x, TravelDistance, l.z), Speed);
		_rTrans.localPosition = l;

		if (_rTrans.localPosition.y == TravelDistance && _once){
			_once = false;
			ActivateChildren();
			GetComponent<AudioSource>().Play(); // audio
			_anim.Play("DropDownDust"); // visual
		}
	}

	public void DeactivateChildren() {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}

	void ActivateChildren() {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(true);
		}
	}
}
