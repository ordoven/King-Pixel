using UnityEngine;
using System;

[ExecuteInEditMode]
public class ReSkinAnimator : MonoBehaviour {

	public string SpriteSheetName;

	private void LateUpdate() {

		var subSprites = Resources.LoadAll<Sprite> ("Enemies/" + SpriteSheetName);

		foreach (var objectRenderer in GetComponentsInChildren<SpriteRenderer>()) 
		{
			var spriteName = objectRenderer.sprite.name;
			var newSprite = Array.Find (subSprites, item => item.name == spriteName);

			if (newSprite)
				objectRenderer.sprite = newSprite;
		}
		
	}
}
