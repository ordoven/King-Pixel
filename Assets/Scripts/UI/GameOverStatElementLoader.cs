using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverStatElementLoader : MonoBehaviour {
	bool _itemsLoaded = false;

    private void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void GameOverStats() {
        StartCoroutine(UiElementsAnim());
    }

	void Update() {
		if (_itemsLoaded) {
			FindObjectOfType<GameManagerBehaviour> ().GetComponent<GameManagerBehaviour> ().Enemies.Clear ();
			FindObjectOfType<GameManagerBehaviour> ().GetComponent<GameManagerBehaviour> ().Weapons.Clear ();
			if (FindObjectOfType<EventSystem> ().GetComponent<EventSystem> ().currentSelectedGameObject == null) {
				FindObjectOfType<EventSystem> ().GetComponent<EventSystem> ().SetSelectedGameObject (GameObject.Find ("Button"));
			}
		}
	}

    IEnumerator UiElementsAnim() {
        for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(true);
			if (transform.GetChild(i).GetComponent<Animator>() != null)
            	transform.GetChild(i).GetComponent<Animator>().Play("GameOverElementPopUp");
            yield return new WaitForSeconds(.5f);
        }
		_itemsLoaded = true;
    }
}
