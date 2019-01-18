using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour {

	[FormerlySerializedAs("wallHitParticle")] [SerializeField]
	private GameObject _wallHitParticle;
	[FormerlySerializedAs("speed")] [SerializeField]
	private float _speed;
	[FormerlySerializedAs("damage")] public int Damage;
    [FormerlySerializedAs("knockBackForce")] public Vector2 KnockBackForce = new Vector2 (10000, 2500);
	[FormerlySerializedAs("destroyOffscreen")] public bool DestroyOffscreen = true; 
	private Rigidbody2D _rbody;
	private Vector2 _direction;

	void Start ()
    {
		_rbody = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
    {
		_rbody.velocity = _direction * _speed;
	}

	void OnBecameInvisible()
    {
		if (DestroyOffscreen)
			DestroyWeapon();
	}

	public void Initialize(Vector2 direction)
    {
		this._direction = direction;
	}

	public void DestroyWeapon(bool particles = false)
    {
		if (particles) {
			Destroy(Instantiate(_wallHitParticle, gameObject.transform.position, Quaternion.identity), 0.5f);
		}
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
    {
		if (other.tag == "Interactable" || other.tag == "Ground") 
			DestroyWeapon(true);
	}
}
