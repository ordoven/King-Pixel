using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [FormerlySerializedAs("walkSpeed_regular")] public float WalkSpeedRegular;
    [FormerlySerializedAs("walkSpeed_aggrovated")] public float WalkSpeedAggrovated;
    [FormerlySerializedAs("jumpHeight")] public float JumpHeight;
    [FormerlySerializedAs("groundCheck")] public Transform GroundCheck;
    [FormerlySerializedAs("groundLayer")] public LayerMask GroundLayer;
    [FormerlySerializedAs("randomColor")] public bool RandomColor;
	[FormerlySerializedAs("colors")] public Color[] Colors;
    public bool PlayerHigherThanEnemy { get; set; }
    [FormerlySerializedAs("canMove")] public bool CanMove = true;

    public float WalkSpeed { get; set; }
    public bool Grounded { get; set; }
    Rigidbody2D _theRigidBody;
    Animator _anim;
    SpriteRenderer _spriteInformation;
    EnemyCombatSystem _combat;

    void Start() {
        _spriteInformation = GetComponent<SpriteRenderer>();
		if (RandomColor && Colors.Length > 0) {
			_spriteInformation.color = Colors[Random.Range(0, Colors.Length)];
        }
        _theRigidBody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _combat = GetComponent<EnemyCombatSystem>();
        WalkSpeed = WalkSpeedRegular;
        PlayerHigherThanEnemy = false;
    }

    void FixedUpdate() {
        Grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.1f, GroundLayer);
        JumpAnimation();
        if (_combat.Target != null && transform.position.x - _combat.Target.transform.position.x > 0f &&
            transform.position.x - _combat.Target.transform.position.x < 0.1f && _combat.Aggrovated) {
            if (_combat.Target.transform.position.y > transform.position.y) {
                PlayerHigherThanEnemy = true;
            } else if (_combat.Target.transform.position.y < transform.position.y) {
                PlayerHigherThanEnemy = true;
            }
        } else {
            if (CanMove && Grounded) {
				_theRigidBody.velocity = new Vector2(WalkSpeed, _theRigidBody.velocity.y);
                _anim.SetFloat("Speed", 1f);
				if (Mathf.Abs (_theRigidBody.velocity.x) < Mathf.Abs (WalkSpeed)) {
					Jump ();
				}
            } else {
                _anim.SetFloat("Speed", 0f);
            }

            PlayerHigherThanEnemy = false;
        }
    }

    void JumpAnimation() {
        if (Grounded)
            _anim.SetFloat("State", 0);
        else if (_theRigidBody.velocity.y > 0.4f)
            _anim.SetFloat("State", 1);
        else if (_theRigidBody.velocity.y < -0.4f)
            _anim.SetFloat("State", 2);
    }

    public void Jump() {
		if (!_spriteInformation.flipX)
			_theRigidBody.velocity = new Vector2(JumpHeight / 2, JumpHeight);
		else
			_theRigidBody.velocity = new Vector2(-JumpHeight / 2, JumpHeight);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyMovementShifter" && _combat.Aggrovated && Grounded) {
            Jump();
        } else if (other.tag == "EnemyMovementShifter" && !_combat.Aggrovated && Grounded) {
			_combat.Turn ();
        }
    }
}
