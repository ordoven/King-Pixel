using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenu : GenericWindow {

    [FormerlySerializedAs("continueButton")] public Button ContinueButton;

    public void Continue() {
        // Last played scene
        print("Last scene loaded");
        SceneManager.LoadScene("");
    }

    public void NewGame() {
        // A whole new game
        StartCoroutine(ChangeLevel(1));
    }

    public void Exit() {
        Application.Quit();
    }

    public IEnumerator ChangeLevel(int levelIndex) {
		yield return new WaitForSeconds(GameObject.Find("Managers").GetComponent<Fading>().BeginFade(1));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + levelIndex);
        GameObject.Find("Managers").GetComponent<Fading>().BeginFade(-1);
    }

    public override void Open() {

        bool canContinue = false;
		ContinueButton.interactable = canContinue;
        base.Open();
    }

    private void Start() {
        Open();
    }

}
