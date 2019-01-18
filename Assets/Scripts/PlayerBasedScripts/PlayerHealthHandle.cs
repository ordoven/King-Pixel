using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthHandle : MonoBehaviour
{

	public int NodeHealth;
	public int CurrentNodeIndex { get; set; }
	[SerializeField] private int _startingNodeAmount;
	public List<int> Health = new List<int>();
	[Space] public float InvincibilityDuration = 0.2f;
	public GameObject HitSound;
	private bool _invincible;
	public bool GameOver { get; set; }
	private float _dur;

	private void Start()
	{
		GameOver = false;
		for (var i = 0; i < _startingNodeAmount; i++)
			Health.Add(NodeHealth);
	}

	private void Update()
	{

		//if (Input.GetKeyDown(KeyCode.V)) {
		//    AddHealthNode();
		//}

		if (Health.Count > 0)
		{
			CurrentNodeIndex = Health.Count - 1;
		}
		else if (Health.Count == 0 && !GameOver)
		{
			GameOver = true;
			Time.timeScale = 0.5f;
			foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
			{
				obj.GetComponent<EnemyController>().CanMove = false;
			}

			gameObject.GetComponent<Animator>().Play("DeathKing");
		}

		if (_dur > 0f)
		{
			_invincible = true;
			StartCoroutine(PlayerAnimatedIndicatorActivation());
			_dur -= Time.deltaTime;
		}
		else
		{
			_invincible = false;
		}
	}

	private IEnumerator PlayerAnimatedIndicatorActivation()
	{
		var descending = true;
		var playerBodyColor = GetComponent<SpriteRenderer>().color;

		while (_dur > 0f)
		{
			playerBodyColor.a = descending ? 0.5f : 1f;
			descending = !descending;
			GetComponent<SpriteRenderer>().color = playerBodyColor;
			yield return new WaitForSeconds(0.1f);
		}

		playerBodyColor.a = 1f;
		GetComponent<SpriteRenderer>().color = playerBodyColor;

		yield return new WaitForEndOfFrame();
	}

	public void AddHealthNode()
	{
		Health.Add(NodeHealth);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("EnemyWeapon") && !GameOver)
		{
			Instantiate(HitSound, transform, false);
			var obj = other.gameObject.GetComponent<Weapon>();
			if (!_invincible)
				ReceiveDamage(obj.Damage, other.transform, obj.KnockBackForce);
			gameObject.GetComponent<Animator>().Play("hitKing");
			obj.DestroyWeapon(true);
		}
		else if (other.CompareTag("Enemy") && !GameOver)
		{
			if (!_invincible)
			{
				Instantiate(HitSound, transform, false);
				var obj = other.GetComponent<EnemyCombatSystem>();

				ReceiveDamage(obj.Suicidal ? obj.Suicide() : obj.RegularDamage, other.transform,
					obj.KnockBackForce);
			}

			gameObject.GetComponent<Animator>().Play("hitKing");
		}
		else if (other.CompareTag("PitFall"))
		{
			Health.Clear();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.CompareTag("Enemy") || GameOver) return;
		if (!_invincible)
		{
			var enemyCombatScript = collision.GetComponent<EnemyCombatSystem>();
			ReceiveDamage(
				enemyCombatScript.Suicidal ? enemyCombatScript.Suicide() : enemyCombatScript.RegularDamage,
				collision.transform, enemyCombatScript.KnockBackForce);
		}

		gameObject.GetComponent<Animator>().Play("hitKing");
	}

	private void ReceiveDamage(int damageAmount, Transform location, Vector2 force)
	{
		if (_dur <= 0f)
			_dur += InvincibilityDuration;
		force *= Time.deltaTime;

		if (location.position.x > transform.position.x)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(force.x * -1f, force.y));
		}
		else
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			GetComponent<Rigidbody2D>().AddForce(force);
		}

		FindObjectOfType<CameraMovement>().GetComponent<CameraMovement>()
			.CameraShake(Random.Range(0.05f, 0.1f), 0.2f);
		StartCoroutine(DoDamage(damageAmount));
	}

	private IEnumerator DoDamage(int damageAmount)
	{
		while (damageAmount > 0 && Health.Count > 0)
		{
			Health[Health.Count - 1]--;
			damageAmount--;
			if (Health[Health.Count - 1] <= 0) Health.RemoveAt(Health.Count - 1);
		}

		yield return new WaitForEndOfFrame();
	}
}
