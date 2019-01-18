using UnityEngine.UI;
using UnityEngine;

public class WeaponUsedStatScript : MonoBehaviour {
    public GameObject LineOfInformation;

    private PlayerHealthHandle _player;
    private GameManagerBehaviour _gm;
    private bool _loaded;

    private void Start()
    {
        _player = FindObjectOfType<PlayerHealthHandle>().GetComponent<PlayerHealthHandle>();
        _gm = FindObjectOfType<GameManagerBehaviour>().GetComponent<GameManagerBehaviour>();
        _loaded = false;
    }

    private void Update()
    {
        if (!_player.GameOver || _loaded) return;
        _loaded = true;
        SpawnStats();
    }

    public void SpawnStats()
    {
        foreach (var obj in _gm.Weapons)
        {
            var thing = Instantiate(LineOfInformation, transform, false);
            thing.transform.GetChild(0).GetComponent<Image>().sprite = obj.Image;
            thing.transform.GetChild(1).GetComponent<Text>().text = obj.Amount.ToString();
        }
    }
}
