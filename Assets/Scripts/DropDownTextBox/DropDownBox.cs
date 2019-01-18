using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class DropDownBox : MonoBehaviour {

	[FormerlySerializedAs("textFile")] public TextAsset TextFile;
	[FormerlySerializedAs("sentences")] public string[] Sentences;

	// Use this for initialization
	void Start () {
		if (TextFile != null) {
			Sentences = (TextFile.text.Split ('\n'));
		}
	}
}