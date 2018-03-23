using UnityEngine;

public class MushroomScript : MonoBehaviour
{

    public enum States
    {
        Static,
        Spawning,
        Moving
    }

    public States State = States.Static;
    private float _velocity = 1.2f;
    private bool _movingRight = true;

    private Rigidbody2D _rigidbody;
//    private Animator _animator;

    private float _spawnSpeed = 0.6f;
    private float _sinceSpawn = 0.0f;
    private Vector3 _spawnPosition;

    // Use this for initialization
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
//        _animator = GetComponent<Animator>();
        _spawnPosition = transform.position;
    }

    public void Spawn()
    {
        State = States.Spawning;
    }
    
    public void Moving()
    {
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        transform.position = _spawnPosition + new Vector3(0, 1, 0);
        State = States.Moving;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (State == States.Spawning) {
            _sinceSpawn += Time.fixedDeltaTime;
            transform.position = _spawnPosition + new Vector3(0, _sinceSpawn / _spawnSpeed) + new Vector3(0, 0, 1);
            if (_sinceSpawn > _spawnSpeed)
                Moving();
        } else {
            var change = new Vector2(_velocity * (_movingRight ? 1 : -1), _rigidbody.velocity.y);
            _rigidbody.velocity = change;
        }
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("entity")) {
            _movingRight = !_movingRight;
        }

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("block")) {
            foreach (var contact in other.contacts) {
                var xComponent = (contact.point - (Vector2) transform.position).x;
                if (xComponent > 0.1) {
                    _movingRight = false;
                } else if (xComponent < -0.1) {
                    _movingRight = true;
                }
            }
        }
    }
}