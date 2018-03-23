using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaScript : MonoBehaviour
{


    private enum State
    {
        Moving,
        Dying
    }

	public bool MovingRight = true;
    private State _state = State.Moving;
	
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private float _velocity = 1.2f;

	// Use this for initialization
	void Start ()
	{
	    _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
	}
    
	
	
    void FixedUpdate()
    {
        if (_state == State.Moving) {
            var change = new Vector2(_velocity * (MovingRight ? 1 : -1), _rigidbody.velocity.y);
            _rigidbody.velocity = change;
        } 
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
    
    public void Die()
    {
        _state = State.Dying;
        _animator.SetTrigger("dying");
        _rigidbody.bodyType = RigidbodyType2D.Static;
        GetComponent<CircleCollider2D>().enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("entity")) {
            MovingRight = !MovingRight;
        }
        
        if (other.gameObject.CompareTag("Player")) {
            var yComponent = (other.contacts[0].point - (Vector2) transform.position).y;
            Debug.Log(yComponent);

            if (yComponent > 0.4) {
                Die();
                other.gameObject.GetComponent<MarioScript>().Bump();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("block")) {
            foreach (var contact in other.contacts) {
                var xComponent = (contact.point - (Vector2) transform.position).x;
                if (xComponent > 0.1) {
                    MovingRight = false;
                } else if (xComponent < -0.1) {
                    MovingRight = true;
                }
            }
        }
    }
}
