using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyCombatSystem : MonoBehaviour {

    [FormerlySerializedAs("suicidal")] public bool Suicidal = false;
    [FormerlySerializedAs("regularDamage")] public int RegularDamage;
	[FormerlySerializedAs("suicideDamage")] public int SuicideDamage;
    [FormerlySerializedAs("knockBackForce")] public Vector2 KnockBackForce;
	[FormerlySerializedAs("enemyLookDistance")] public float EnemyLookDistance;
	[FormerlySerializedAs("attackDistance")] public float AttackDistance;
    [FormerlySerializedAs("usingWeapon")] public bool UsingWeapon = false;
	[FormerlySerializedAs("defaultWeapon")] public GameObject DefaultWeapon;
    [FormerlySerializedAs("fireRateDelay")] [SerializeField]
	private float _fireRateDelay;
	[FormerlySerializedAs("playerLayer")] public LayerMask PlayerLayer;
	[FormerlySerializedAs("suspiciousModeParticles")] public GameObject SuspiciousModeParticles;
    public bool Aggrovated { get; set; }
    public GameObject Target { get; set; }
    private float _nextFire = -1.0f;
    private float _fpsTargetDistance;
    private bool _inLineOfSight;
    EnemyController _enemy;
	SpriteRenderer _spriteInformation;

	void Start () {
        Target = GameObject.FindGameObjectWithTag("Player");

        _spriteInformation = GetComponent<SpriteRenderer>();
        _enemy = gameObject.GetComponent<EnemyController>();

        if (_spriteInformation.flipX) {
            _enemy.WalkSpeedAggrovated *= -1;
            _enemy.WalkSpeedRegular *= -1;
            _enemy.WalkSpeed *= -1;
        }

        Aggrovated = false;
		_inLineOfSight = false;
		_nextFire = Time.time;
	}
	
	void Update () {
		_inLineOfSight = Raycasting();
        if (_inLineOfSight) 
			Aggrovated = true;
	}

	void FixedUpdate() {
        if (Target == null) {
            try { Target = GameObject.FindGameObjectWithTag("Player"); }
            catch { return; }
        } else {
            _fpsTargetDistance = Vector3.Distance(Target.transform.position, transform.position);

            if (Aggrovated) {
                _enemy.WalkSpeed = _enemy.WalkSpeedAggrovated;
				SuspiciousModeParticles.SetActive(false);

				if (transform.position.x < Target.transform.position.x && _enemy.WalkSpeed < 0)
					Turn ();
				else if (transform.position.x > Target.transform.position.x && _enemy.WalkSpeed > 0)
					Turn ();
				
                if (_enemy.PlayerHigherThanEnemy && Random.Range(0, 50) == 0 && _enemy.Grounded)
                    _enemy.Jump();

                if (UsingWeapon)
                    Attack();
            }
            else {
                _enemy.WalkSpeed = _enemy.WalkSpeedRegular;

                if (_fpsTargetDistance <= EnemyLookDistance)
                    SuspiciousModeParticles.SetActive(true);
                else
                    SuspiciousModeParticles.SetActive(false);

                if (_fpsTargetDistance <= AttackDistance && _inLineOfSight) {
                    if (Target.transform.localPosition.x - transform.localPosition.x < 0 &&
						_enemy.WalkSpeed > 0 && !_enemy.PlayerHigherThanEnemy) 
						Turn();
                    else if (Target.transform.localPosition.x - transform.localPosition.x > 0 &&
						_enemy.WalkSpeed < 0 && !_enemy.PlayerHigherThanEnemy) 
						Turn();
                    Aggrovated = true;
                } else if (_enemy.Grounded) {
					Aggrovated = false;
                }
            }
        }
	}

	void Attack() {
		if (Time.time >= _nextFire && _inLineOfSight) {
			_nextFire = Time.time + _fireRateDelay;
			StartCoroutine(ThrowWeapon(Random.Range(0f, 1f)));
		}
	}

	public virtual int Suicide() {
		EnemyHealthHandle handler = gameObject.GetComponent<EnemyHealthHandle>();
		handler.Armor = 0;
		handler.Health = 0;
		return SuicideDamage;
	}

	private IEnumerator ThrowWeapon(float delay) {
		_nextFire += delay;
		yield return new WaitForSeconds (delay);
        gameObject.GetComponent<Animator>().Play("SlashKing");
		if (!_spriteInformation.flipX) { 
			var position = new Vector3(transform.position.x+0.7f, transform.position.y, transform.position.z);
			var tmp = Instantiate(DefaultWeapon, position, Quaternion.Euler(new Vector3(0, 0, -90)));
			tmp.GetComponent<Weapon>().Initialize(Vector2.right);
            tmp.tag = "EnemyWeapon";
        } else {
			var position = new Vector3(transform.position.x-0.7f, transform.position.y, transform.position.z);
			var tmp = Instantiate(DefaultWeapon, position, Quaternion.Euler(new Vector3(0, 0, 90)));
			tmp.GetComponent<Weapon>().Initialize(Vector2.left);
            tmp.tag = "EnemyWeapon";
		}

    } 

	public void Turn() {
		_spriteInformation.flipX = !_spriteInformation.flipX;
        _enemy.WalkSpeedAggrovated *= -1;
        _enemy.WalkSpeedRegular *= -1;
        _enemy.WalkSpeed *= -1;
	}
	
	bool Raycasting() { 
		Vector3 point = transform.position;

		if (_spriteInformation.flipX)
			point -= new Vector3(AttackDistance, 0f, 0f);
		else
			point += new Vector3(AttackDistance, 0f, 0f);
        //Debug.DrawLine(transform.position, point, Color.red);
		return Physics2D.Linecast(transform.position, point, PlayerLayer);
	}
}
