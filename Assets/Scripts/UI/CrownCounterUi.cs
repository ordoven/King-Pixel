using UnityEngine;
using UnityEngine.UI;

public class CrownCounterUi : MonoBehaviour {
	int CrownAmount { get; set; }
	int _maxAmount, _currentAmount;
	Text _txt;

	void Start () {
		_txt = GetComponent<Text> ();
		_maxAmount = GameObject.FindGameObjectsWithTag ("Collectable").Length;
		CrownAmount = _maxAmount;
		_currentAmount = 0;
		_txt.text = _currentAmount.ToString () + "/" + _maxAmount.ToString ();
	}

	void Update () {
		if (CrownAmount != GameObject.FindGameObjectsWithTag ("Collectable").Length) {
			_currentAmount++;
			_txt.text = _currentAmount.ToString () + "/" + _maxAmount.ToString ();
			CrownAmount--;
		}
	}
}
