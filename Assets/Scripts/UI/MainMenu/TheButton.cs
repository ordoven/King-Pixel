using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Serialization;

public class TheButton : MonoBehaviour {
	private bool _selected;
	[FormerlySerializedAs("indicatorPrefab")] public GameObject IndicatorPrefab;
	private GameObject _dik;

	void Start () {
		_selected = false;
	}
	
	void Update () {
		if (EventSystem.current.currentSelectedGameObject == gameObject && !_selected) {
			_selected = true;
			_dik = Instantiate(IndicatorPrefab, transform, false);
			IndicatorAnimation();
		}
		else if (EventSystem.current.currentSelectedGameObject != gameObject && _selected) {
			_selected = false;
			Destroy(_dik);
		}
	}

	void IndicatorAnimation() {
		StartCoroutine(Anim());
	}

	private IEnumerator Anim() {
		Color temp = _dik.GetComponent<Image>().color;
		temp.a = 0;
		_dik.GetComponent<Image>().color = temp;
		while (_dik != null && _dik.GetComponent<Image>().color.a != 1f)
		{
			temp.a += 0.1f;
			_dik.GetComponent<Image>().color = temp;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
