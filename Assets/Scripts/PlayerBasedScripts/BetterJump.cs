using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BetterJump : MonoBehaviour {

	public float FallMultiplier = 2f;
	public float LowJumpMultiplier = 1.2f;
	private Rigidbody2D _rBody;

	private void Start ()
	{
		_rBody = GetComponent<Rigidbody2D> ();	
	}

	private void Update ()
	{
		if (_rBody.velocity.y < 0)
		{
			_rBody.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
		} 
		else if (_rBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
		{
			_rBody.velocity += Vector2.up * Physics2D.gravity.y * (LowJumpMultiplier - 1) * Time.deltaTime;
		}
	}
}
