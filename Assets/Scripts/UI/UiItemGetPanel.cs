using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class UiItemGetPanel : MonoBehaviour {

	[FormerlySerializedAs("itemImage")] public Sprite ItemImage;
	[FormerlySerializedAs("durability")] public int Durability;

	private Image _imageObject;
	private Text _textObject;
	private Color _defaultColor;

	void Start(){
		_imageObject = transform.Find ("Image").GetComponent<Image>();
		_textObject = transform.Find ("Text").GetComponent<Text>();
		_defaultColor = gameObject.GetComponent<Image> ().color;
		CleanUp ();
	}

	void Set(Sprite itemImage, int durability) {
		GetComponent<Image> ().color = _defaultColor;
		_imageObject.sprite = itemImage;
		_imageObject.color = new Vector4 (1f, 1f, 1f, 1f);
		_textObject.text = durability.ToString() + "x";
	}

	void CleanUp(){
		GetComponent<Image>().color = new Vector4 (1f, 1f, 1f, 0f);
		_imageObject.sprite = null;
		_imageObject.color = new Vector4 (1f, 1f, 1f, 0f);
		_textObject.text = "";
	}

	public void EnableItemUiPanel(Sprite itemImage, int durability, float duration){
		Set (itemImage, durability);
		StartCoroutine (CountDown(duration));
	}

	private IEnumerator CountDown(float duration){
		yield return new WaitForSeconds (duration);
		CleanUp ();
	}
}
