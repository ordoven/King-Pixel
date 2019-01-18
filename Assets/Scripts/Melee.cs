using UnityEngine;
using UnityEngine.Serialization;

public class Melee : MonoBehaviour {

    public int Damage;
    public Vector2 KnockBackForce;

    [SerializeField] private float _destructionDelay;

    private Transform _player;
    private float _offset;

    private void Start ()
    {
        Destroy(gameObject, _destructionDelay);
	}

    private void Update()
    {
        transform.position = new Vector3(_player.position.x + _offset, _player.position.y, _player.position.z);
    }

    public void SetMeleeAttack(Transform obj, float offset)
    {
        _player = obj;
        _offset = offset;
    }
}
