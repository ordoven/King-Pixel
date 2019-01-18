using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class UiHealth : MonoBehaviour {
    [SerializeField] private GameObject _healthNodePrefab;
	private readonly List<GameObject> _healthNodeList = new List<GameObject>();

	private PlayerHealthHandle _playerHealth;

    private void Start () {
		_playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthHandle>();
	}

    private void Update () {
        if (_playerHealth == null) 
			return;
        UpdateUiHealth();
    }

    private void UpdateUiHealth() {
        while (_healthNodeList.Count != _playerHealth.Health.Count) {
            if (_healthNodeList.Count < _playerHealth.Health.Count)
                CreateHealthNode();
            else
                RemoveHealthNode(_playerHealth.CurrentNodeIndex);
        }
        if (_healthNodeList.Count == 0)
            return;
        _healthNodeList[_playerHealth.CurrentNodeIndex].transform.GetChild(0).gameObject.GetComponent<Image>().fillAmount = (float)(Convert.ToDecimal(_playerHealth.Health[_playerHealth.CurrentNodeIndex]) / Convert.ToDecimal(_playerHealth.NodeHealth));
    }

    private void CreateHealthNode() {
        var obj = Instantiate(_healthNodePrefab, transform, false);
        _healthNodeList.Add(obj);
    }

    private void RemoveHealthNode(int index) {
        Destroy(_healthNodeList[index]);
        _healthNodeList.RemoveAt(index);
    }
}
