using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class WeaponHandle : MonoBehaviour {

    [FormerlySerializedAs("currentWeaponPrefab")] public GameObject CurrentWeaponPrefab;
    [FormerlySerializedAs("currentWeaponDurability")] public int CurrentWeaponDurability;
    [FormerlySerializedAs("fireRate")] public float FireRate;
    [FormerlySerializedAs("throwSounds")] [Space]
    public AudioClip[] ThrowSounds;
    [FormerlySerializedAs("audioSourceObject")] public GameObject AudioSourceObject;


    private Animator _playerAnimation;
	private PlayerController _player;
    private bool _canShoot;

	void Start() {
		_playerAnimation = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
        _canShoot = true;
	}

	void Update () { // Button for Weapon Toss
		if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.JoystickButton2)) && _player.CanMove && !_player.OnLadder &&
			CurrentWeaponDurability > 0 && CurrentWeaponPrefab.tag == "Weapon" && _canShoot)
        {
			ThrowWeapon(0);
            var aud = AudioSourceObject.GetComponent<AudioSource>();
            aud.clip = ThrowSounds[Random.Range(0, ThrowSounds.Length)];
            aud.Play();
            StartCoroutine(Delay(_canShoot));
		}
	}

    IEnumerator Delay(bool condition) {
        _canShoot = false;
        yield return new WaitForSeconds(FireRate);
        _canShoot = true;
    }

	void ThrowWeapon(int value) {
		_playerAnimation.Play("SlashKing");
        FindObjectOfType<GameManagerBehaviour>().
            GetComponent<GameManagerBehaviour>().
            AddWeapon(CurrentWeaponPrefab.GetComponent<SpriteRenderer>().sprite, 1);
        var position = transform.position;

        if (!gameObject.GetComponent<SpriteRenderer>().flipX) {

			position.x += 0.2f;
			var tmp = Instantiate(CurrentWeaponPrefab, position, Quaternion.Euler(new Vector3(0, 0, -90)));
			tmp.GetComponent<Weapon>().Initialize(Vector2.right);
            tmp.GetComponent<SpriteRenderer>().flipX = true;
        }
        else {
			position.x -= 0.2f;
            var tmp = Instantiate(CurrentWeaponPrefab, position, Quaternion.Euler(new Vector3(0, 0, 90)));
            tmp.GetComponent<Weapon>().Initialize(Vector2.left);
            tmp.GetComponent<SpriteRenderer>().flipX = false;
        }
		CurrentWeaponDurability--;
	}
}
