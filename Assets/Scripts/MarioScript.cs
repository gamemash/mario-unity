using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioScript : MonoBehaviour
{


	private float _fallMultiplier = 8.0f;
	private float _lowJumpMultiplier = 8.0f;
	private float JumpVelocity = 10.0f;

	private float _sinceJump = 1.0f;
	private float _delayedJumpTime = 0.2f;

	private float _acceleration = 20.0f;

	private Rigidbody2D _rigidbody;
	private bool _grounded;
	private Animator _animator;
	
	private AudioSource _audioSource;
	private AudioClip[] _clips;

	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
		_audioSource = GetComponent<AudioSource>();
		_clips = new AudioClip[]
		{
			Resources.Load<AudioClip>("Audio/smb_jump-small")
		};
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetButton("Horizontal")) {
			_rigidbody.velocity += new Vector2(Input.GetAxis("Horizontal"), 0) * _acceleration * Time.fixedDeltaTime;
		}
		

		if (Input.GetButton("Jump")) {
			if (_grounded) {
				Jump();
			}


			if (_sinceJump < _delayedJumpTime) {
				_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, JumpVelocity);
				_sinceJump += Time.fixedDeltaTime;
			}

		}


		if (_rigidbody.velocity.y < 0) {
			_rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;
		} else if (_rigidbody.velocity.y > 0 && !Input.GetButton("Jump")) {
			_rigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
		}
		
		
		_animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity.x));
		_setDirection();

		_animator.SetBool("sliding", (int)Mathf.Sign(_rigidbody.velocity.x) != (int)Mathf.Sign(Input.GetAxis("Horizontal")));
	}

	public void Jump()
	{
		_audioSource.clip = _clips[0];
		_audioSource.Play();
		_animator.SetTrigger("jumping");
		_sinceJump = 0.0f;
	}

	public void Bump()
	{
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Abs(_rigidbody.velocity.y) * 1.5f);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("block")) {
			var verticalDistance = other.contacts[0].point.y - transform.position.y;
			if (verticalDistance < 0.2) {
				_grounded = true;
				_animator.SetBool("grounded", _grounded);
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("block")) {
			_grounded = false;
            _animator.SetBool("grounded", _grounded);
		}
	}

	private void _setDirection()
	{

		var scale = transform.localScale;
		if (Input.GetAxis("Horizontal") > 0) {
			scale.x = Mathf.Abs(scale.x);
		} else if (Input.GetAxis("Horizontal") < 0) {
			scale.x = -Mathf.Abs(scale.x);
		}

		transform.localScale = scale;
	}
}
