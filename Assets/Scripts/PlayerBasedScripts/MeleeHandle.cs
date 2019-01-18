using System.Collections;
using UnityEngine;

public class MeleeHandle : MonoBehaviour {

    [SerializeField] private GameObject _meleePrefab;
    [SerializeField] private float _offset;
    [Space] public AudioClip MeleeSound;
    public GameObject AudioSourceObject;
    private Animator _playersAnimator;
    private PlayerController _player;
    private bool _delay;

    private void Start()
    {
        _playersAnimator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
        _delay = false;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.N) || !_player.CanMove || _player.OnLadder || _delay) return;
        StartCoroutine(Melee(0.5f));
        print("Punch");
    }

    private void ThrowAPunch()
    {
        _playersAnimator.Play("punchKing"); // "SlashKing"
        PlayPunchSound();

        var position = transform.position;
        if (!gameObject.GetComponent<SpriteRenderer>().flipX)
        {
            position.x += _offset;
            var obj = Instantiate(_meleePrefab, position, Quaternion.identity);
            obj.GetComponent<Melee>().SetMeleeAttack(transform, _offset);
        }
        else
        {
            position.x -= _offset;
            var obj = Instantiate(_meleePrefab, position, Quaternion.identity);
            obj.GetComponent<Melee>().SetMeleeAttack(transform, -_offset);
            obj.GetComponent<SpriteRenderer>().flipX = !obj.GetComponent<SpriteRenderer>().flipX;
        }
    }

    private void PlayPunchSound()
    {
        var sfxAudio = AudioSourceObject.GetComponent<AudioSource>();
        if (sfxAudio.isPlaying) return;
        sfxAudio.clip = MeleeSound;
        sfxAudio.Play();
    }
    
    private IEnumerator Melee(float delay)
    {
        _delay = true;
        ThrowAPunch();
        yield return new WaitForSeconds(delay);
        _delay = false;
    }
}
