using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UiObjectOptimize : MonoBehaviour {

    [FormerlySerializedAs("delayBeforeDestruction")] public float DelayBeforeDestruction;

	// Use this for initialization
	void Awake () {
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(
            gameObject.GetComponent<RectTransform>().sizeDelta.x,
            gameObject.GetComponent<RectTransform>().sizeDelta.y
            );
        Destroy(gameObject, DelayBeforeDestruction);
    }
}
