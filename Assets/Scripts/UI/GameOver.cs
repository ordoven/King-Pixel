using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
    private PlayerHealthHandle _playersHealth;
    private Image _panel;
    private bool _gameOver, _uiElementsVisible;
    public GameObject GameOver1;

    private void Start () {
        _gameOver = false;
        _uiElementsVisible = false;
        _playersHealth = FindObjectOfType<PlayerHealthHandle>().GetComponent<PlayerHealthHandle>();
        _panel = GetComponent<Image>();
        _panel.color = new Vector4(_panel.color.r, _panel.color.g, _panel.color.b, 0f);
        GameOver1.SetActive(false);
    }

    private void Update () {
        if (!_playersHealth.GameOver || _gameOver) return;
        _gameOver = true;
        StartCoroutine(FadeInGameOverAnim());
    }

    private IEnumerator FadeInGameOverAnim() {
        var d = _panel.color.a;
        while (_panel.color.a != 1f) {
            d += 0.025f;
            _panel.color = new Vector4(_panel.color.r, _panel.color.g, _panel.color.b, d);
            if (_panel.color.a > .9f && !_uiElementsVisible){
                _uiElementsVisible = true;
                GameOver1.SetActive(true);
                GameOver1.GetComponent<Animator>().Play("GameOverTitle");
				FindObjectOfType<InventorySystem> ().Items.Clear ();
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
