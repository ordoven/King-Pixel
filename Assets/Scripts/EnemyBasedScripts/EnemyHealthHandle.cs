using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealthHandle : MonoBehaviour {
	[FormerlySerializedAs("health")] public int Health;
	[FormerlySerializedAs("maxHealth")] public int MaxHealth;
	[FormerlySerializedAs("armor")] public int Armor;
	[FormerlySerializedAs("hitSound")] public GameObject HitSound;
	[FormerlySerializedAs("deathSound")] public GameObject DeathSound;

    private GameManagerBehaviour _gm;
    private EnemyCombatSystem _combat;
    private Sprite _enemyPicture;
	bool _dead;

    private void Awake() {
        _enemyPicture = GetComponent<SpriteRenderer>().sprite;
		_dead = false;
    }

    private void Start() {
        _gm = FindObjectOfType<GameManagerBehaviour>().GetComponent<GameManagerBehaviour>();
        _combat = GetComponent<EnemyCombatSystem>();
    }

    void FixedUpdate() {
		if (Health <= 0f && Armor <= 0f && !_dead) {
			_dead = true;
            _gm.AddEnemy(_enemyPicture, 1);
			if (_combat.Suicidal) {
				GetComponent<Animator> ().Play ("ExplosionKing");
				GetComponent<AudioSource> ().Play ();
			}
			else {
				GetComponent<Animator>().Play("DeathKing");
				Instantiate (DeathSound, transform, false);
			}
			Destroy(gameObject, .25f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Weapon")
        {
			Instantiate (HitSound, transform, false);
            ReceiveDamage(other.gameObject.GetComponent<Weapon>().Damage, other.transform, other.gameObject.GetComponent<Weapon>().KnockBackForce);
            gameObject.GetComponent<Animator>().Play("hitKing");
            other.gameObject.GetComponent<Weapon>().DestroyWeapon(true);
            _combat.Aggrovated = true;
        }
        else if (other.tag == "Melee")
        {
			Instantiate (HitSound, transform, false);
            ReceiveDamage(other.gameObject.GetComponent<Melee>().Damage, other.transform, other.gameObject.GetComponent<Melee>().KnockBackForce);
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Animator>().Play("hitKing");
            _combat.Aggrovated = true;
        }
	}

	void ReceiveDamage(int damageAmount, Transform location, Vector2 force) {
        var og = GetComponent<EnemyController>();
        og.CanMove = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        if (location.position.x > transform.position.x) {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x * -1f, force.y * 8));
        }
        else {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x, force.y * 8));
        }

        StartCoroutine(DoDamage(damageAmount, og));
	}

	private IEnumerator DoDamage(int damageAmount, EnemyController og) {
        Health -= damageAmount;
        yield return new WaitForSeconds(.1f); // hard-coded delay
        og.CanMove = true;
    }
}
