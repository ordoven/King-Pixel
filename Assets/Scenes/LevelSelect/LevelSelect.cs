using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {

	public GameObject[] Worlds;
	public EventSystem EventSystem;
	public int ActiveWorld { private get; set; }


	private void Start ()
	{
		ActiveWorld = 0;
		foreach (var obj in Worlds) {
			obj.SetActive (false);
		}
		Worlds [ActiveWorld].SetActive (true);
	}

	private void Update()
	{
		if (EventSystem.currentSelectedGameObject == null) {
			EventSystem.SetSelectedGameObject(GameObject.Find("Button"));
		}		
	}

	public void ChangeWorld(bool increases)
	{
		Worlds [ActiveWorld].SetActive (false);
		if (increases)
			ActiveWorld++;
		else
			ActiveWorld--;
		Worlds [ActiveWorld].SetActive (true);
		EventSystem.SetSelectedGameObject (Worlds [ActiveWorld].transform.GetChild (0).gameObject);
	}

	public void GotoLevel(int buildIndex) {
		StartCoroutine(ChangeLevel(buildIndex));
	}

	private static IEnumerator ChangeLevel(int levelIndex) {
		yield return new WaitForSeconds(GameObject.Find("Managers").GetComponent<Fading>().BeginFade(1));
		SceneManager.LoadScene(levelIndex);
		
	}
}

