using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour {
	public Sprite OpenedChestSpriteGlow;
    public Sprite OpenedChestSpriteNormal;
	public GameObject RewardItem;
	public int RewardDurability;

	private bool _waitForPress;
	private SpriteRenderer _chestSpriteInformation;
	private Sprite _rewardItemSprite;
    private AudioSource _sfxAudio;

	private void Start() {
        _sfxAudio = GetComponent<AudioSource>();
		_chestSpriteInformation = GetComponent<SpriteRenderer>();
		_rewardItemSprite = RewardItem.GetComponent<SpriteRenderer> ().sprite;
		_waitForPress = false;
	}

	private void Update() {
		if (_waitForPress && Input.GetKeyDown (KeyCode.E)) {
			OpenChest ();
		}
	}

	private void ChestAnimation() {
        StartCoroutine(Anim());
	}

    private IEnumerator Anim() {
        _chestSpriteInformation.sprite = OpenedChestSpriteGlow;
        yield return new WaitForSeconds(3f);
        _chestSpriteInformation.sprite = OpenedChestSpriteNormal;
    }

	private void OpenChest() {
		var playerWeaponInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponHandle> ();
		var inventory = GameObject.FindGameObjectWithTag ("Inventory").GetComponent<InventorySystem> ();

		if (_chestSpriteInformation.sprite == OpenedChestSpriteGlow ||
		    _chestSpriteInformation.sprite == OpenedChestSpriteNormal) return;
		if (!(inventory.IsWeaponEquipped (RewardItem, RewardDurability))) {
			playerWeaponInfo.CurrentWeaponPrefab = RewardItem;
			playerWeaponInfo.CurrentWeaponDurability = RewardDurability;
		}
		_sfxAudio.Play();
		ChestAnimation ();
		GameObject.FindGameObjectWithTag ("ItemUIHandler").
			GetComponent<UiItemGetPanel> ().
			EnableItemUiPanel (_rewardItemSprite, RewardDurability, 3f);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			_waitForPress = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.CompareTag("Player")) {
			_waitForPress = false;
		}
	}
}
