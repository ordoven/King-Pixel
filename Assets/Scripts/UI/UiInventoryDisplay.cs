using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiInventoryDisplay : MonoBehaviour {

    private InventorySystem _inventory;
    public GameObject ItemNodePrefab;
    public List<ItemNode> Items;

	private void Start () {
        _inventory = FindObjectOfType<InventorySystem>().GetComponent<InventorySystem>();
	}

	private void Update () {
		Selected();
		Add();
        UpdateInformation();
	}

	private void FixedUpdate() {
		foreach (var item in Items) {
			if (item.Dur != 0) continue;
			Destroy(item);
			// display new item selection
			var index = Items.IndexOf(item);
			if (Items.Count != 1) {
				if (Items.Count == 2 && index == 0)
					SelectionSelect(index + 1);
				else if (index == Items.Count - 1)
					SelectionSelect(index - 1);
				else
					SelectionSelect(Items.Count - 1);
			}
			Items.Remove(item);
		}
	}

	public void AddItem(Sprite image, int durability) {
        var node = Instantiate(ItemNodePrefab, transform, false);
        node.transform.SetParent(gameObject.transform);
        node.GetComponent<ItemNode>().Dur = durability;
        node.GetComponent<Image>().sprite = image;
		node.transform.Find("Selected").gameObject.SetActive(false);
        Items.Add(node.GetComponent<ItemNode>());
		SelectionSelect(Items.Count - 1);
    }

	private void Selected()
	{
		const int index = 49;
		for (var i = 0; i < Items.Count; i++) {
			if (Input.GetKeyDown((KeyCode)index + i) && Items[i] != null) {
				SelectionSelect(i);
			}
		}
	}

	private void SelectionSelect(int selection) {
		foreach (var item in Items)
			item.transform.Find("Selected").gameObject.SetActive(false);

		Items[selection].transform.Find("Selected").gameObject.SetActive(true);

		StartCoroutine(
			SelectionAnimation(Items[selection].transform.Find("Selected").
			gameObject.GetComponent<Image>()) 
			);
	}

	private static IEnumerator SelectionAnimation(Image img) {
		img.fillAmount = 0f;
		while (img.fillAmount != 1f) {
			img.fillAmount = Mathf.MoveTowards(img.fillAmount, 1f, 0.1f);
			yield return new WaitForSeconds(.01f);
		}
		yield return new WaitForEndOfFrame();
	}

	private void Add()
    {
        foreach (var item in _inventory.Items)
        {
            var inc = false;
            foreach (var i in Items) {
	            if (i.TheImage.sprite != item.Pic) continue;
	            inc = true;
	            break;
            }
            if (!inc && item.Durability > 0)
                AddItem(item.Pic, item.Durability);
        }
    }

	private void UpdateInformation() {
        foreach (var item in _inventory.Items) foreach (var i in Items) {
            if (i.GetComponent<Image>().sprite == item.Pic && i.Dur != item.Durability)
                i.Dur = item.Durability;
        }
    }
}
