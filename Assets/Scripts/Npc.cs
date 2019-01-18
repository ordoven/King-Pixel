using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Npc : MonoBehaviour {

    [FormerlySerializedAs("walkSpeed")] public float WalkSpeed;
    [FormerlySerializedAs("waitTimeRange")] public Vector2 WaitTimeRange;

    private Animator _anim;
    private bool _canRun;
    private TextBoxManager _textBox;
    private SpriteRenderer _spriteInformation;

	void Start ()
    {
        _canRun = true;
        _textBox = GameObject.Find("TextBoxManager").GetComponent<TextBoxManager>();
        _spriteInformation = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
	}

	void Update ()
    {
        if (_canRun && !_textBox.Pressed)
        {
            _anim.SetFloat("Speed", 1f);
            Running();
            if (Random.Range(0, 201) == 1)
            {
                StartCoroutine(StopAndLookAround());
            }
        }
        else
        {
            if (_textBox.Pressed)
            {
                if (GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x)
                    _spriteInformation.flipX = false;
                else
                    _spriteInformation.flipX = true;
            }
            _anim.SetFloat("Speed", 0f);
        }
	}

    void Running()
    {
        var pos = transform.position;

        if (!_spriteInformation.flipX) // Right
        {
            pos.x += WalkSpeed * Time.deltaTime;

        }
        else
        {
            pos.x -= WalkSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }

    private IEnumerator StopAndLookAround()
    {
        _canRun = false;
        yield return new WaitForSeconds(Random.Range(WaitTimeRange.x, WaitTimeRange.y));
        _canRun = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyMovementShifter")
        {
            Turn();
        }
    }

    void Turn()
    {
        _spriteInformation.flipX = !_spriteInformation.flipX;
    }

}
