using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemThing{
    public readonly GameObject Item;
    public int Durability;
    public Sprite Pic;

    public ItemThing(GameObject it, int dur)
    {
        Item = it;
        Durability = dur;
        Pic = it.GetComponent<SpriteRenderer>().sprite;
    }
}

public class InventorySystem : MonoBehaviour {
    private GameObject _player;
    private WeaponHandle _playerWeaponSettings;
    public List<ItemThing> Items = new List<ItemThing>();

    private void Start ()
    {
        _player = GameObject.FindGameObjectWithTag ("Player");
        if (_player != null) _playerWeaponSettings = _player.GetComponent<WeaponHandle> ();
    }

    private void Update ()
    {
        CheckIfEverythingIsFine();

        if (SceneManager.GetActiveScene ().buildIndex == 0 && Items.Count > 0) { // Main-menu
            Items.Clear();	
        }

        if (_player == null) _player = GameObject.FindGameObjectWithTag ("Player");
        else
        {
            UpdateCurrentWeaponInformation ();
            PendingWeaponChange ();
            CheckForNewWeapon();
        }

    }

    // Method used in Chest Behaviour.cs
    public bool IsWeaponEquipped(GameObject weapon, int durability)
    {
        CheckIfEverythingIsFine();
        if (weapon == _playerWeaponSettings.CurrentWeaponPrefab)
        {
            _playerWeaponSettings.CurrentWeaponDurability += durability;
            return true;
        }

        foreach (var item in Items) if (item.Item == weapon)
        {
            item.Durability += durability;
            return true;
        }

        return false;
    }

    private void CheckIfEverythingIsFine()
    {
        if (_playerWeaponSettings != null) return;
        try { _playerWeaponSettings = _player.GetComponent<WeaponHandle>(); }
        catch
        {
            // ignored
        }
    }


    private void PendingWeaponChange()
    {
        const int index = 49;
        for (var i = 0; i < Items.Count; i++) {
            if (!Input.GetKeyDown((KeyCode) index + i) || Items[i] == null) continue;
            _playerWeaponSettings.CurrentWeaponPrefab = Items[i].Item;
            _playerWeaponSettings.CurrentWeaponDurability = Items[i].Durability;
        }
    }

    private void CheckForNewWeapon()
    {
        foreach (var item in Items)
            if (item.Item == _playerWeaponSettings.CurrentWeaponPrefab)
                return;

        if (_playerWeaponSettings.CurrentWeaponPrefab == null ||
            !_playerWeaponSettings.CurrentWeaponPrefab.CompareTag("Weapon") ||
            _playerWeaponSettings.CurrentWeaponDurability <= 0) return;
        Items.Add(new ItemThing(_playerWeaponSettings.CurrentWeaponPrefab,
            _playerWeaponSettings.CurrentWeaponDurability));
    }

    private void UpdateCurrentWeaponInformation()
    {
        foreach (var item in Items)
        {
            if (item.Item != _playerWeaponSettings.CurrentWeaponPrefab) continue;
            if (item.Durability == 0) {
                Items.Remove(item);
                if (Items.Count <= 0) continue;
                _playerWeaponSettings.CurrentWeaponPrefab = Items[Items.Count - 1].Item;
                _playerWeaponSettings.CurrentWeaponDurability = Items[Items.Count - 1].Durability;
            }
            else item.Durability = _playerWeaponSettings.CurrentWeaponDurability;
        }
    }
}
