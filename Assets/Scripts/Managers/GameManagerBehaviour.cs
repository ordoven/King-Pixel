using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class ItemInformation {
    public Sprite Image;
    public int Amount;

    public ItemInformation(Sprite img, int am) {
        Image = img;
        Amount = am;
    }
}

public class GameManagerBehaviour : MonoBehaviour {
	public List<ItemInformation> Enemies = new List<ItemInformation>();
	public List<ItemInformation> Weapons = new List<ItemInformation>();

	private PlayerHealthHandle _playerStatus;
	public GameObject EscapeMenu;

	private void Start() {
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			_playerStatus = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealthHandle> ();
		}
	}

	private void Update() {
		if (GameObject.FindGameObjectWithTag ("esc_menu") != null && EscapeMenu == null) {
			EscapeMenu = GameObject.FindGameObjectWithTag ("esc_menu");
			EscapeMenu.SetActive (false);
		}

		if (GameObject.FindGameObjectWithTag("Player") == null) return;
		_playerStatus = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerHealthHandle> ();
		
		if (_playerStatus.GameOver || !FindObjectOfType<CameraMovement>().SmoothFollow || !Input.GetKeyDown(KeyCode.Escape)) return;
		
		EscapeMenu.SetActive (!EscapeMenu.activeSelf);
		GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController>().enabled = !EscapeMenu.activeSelf; 
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		
		if (EscapeMenu.activeSelf) {
			FindObjectOfType<EventSystem> ().GetComponent<EventSystem> ().SetSelectedGameObject (EscapeMenu.transform.Find("Button").gameObject);
		}
	}

    public void AddEnemy(Sprite image, int amount) {
        if (!IsIn(image, amount, Enemies)) {
            Enemies.Add(new ItemInformation(image, amount));
        }
    }

    public void AddWeapon(Sprite image, int amount) {
        if (!IsIn(image, amount, Weapons)) {
            Weapons.Add(new ItemInformation(image, amount));
        }
    }

	private static bool IsIn(Object image, int amount, IEnumerable<ItemInformation> arr) {
        foreach (var item in arr) {
	        if (item.Image != image) continue;
	        item.Amount += amount;
	        return true;
        }
        return false;
    }
}
