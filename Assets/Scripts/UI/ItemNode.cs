using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemNode : MonoBehaviour {

    public Image TheImage { get; set; }
    [FormerlySerializedAs("theDurability")] public Text TheDurability;
    [FormerlySerializedAs("dur")] public int Dur;


    private void Start ()
    {
        TheImage = GetComponent<Image>();
        TheImage.color = new Vector4(1f, 1f, 1f, 0f);
        TheDurability.text = "";
        Dur = 1;
	}

    private void Update()
    {
        ColorToggle();
    }

    private void ColorToggle()
    {
        if (TheImage.sprite == null) return;
        TheImage.color = new Vector4(1f, 1f, 1f, 1f);
        TheDurability.text = Dur + "x";
    }
}
