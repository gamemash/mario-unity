using UnityEngine;

public class PointScript : MonoBehaviour {

	public enum Points
	{
		OneUp = 0,
		P100  = 1,
		P200  = 2,
		P400  = 3,
		P500  = 4,
		P800  = 5,
		P1000 = 6
	}

	public Points Point;
	private SpriteRenderer _spriteRenderer;
	private Sprite[] _sprites;


	private float _height = 2.0f;
	private float _duration = 1.7f;
	private float _sinceStart = 0.0f;

	private Vector2 _position;

	// Use this for initialization
	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_sprites = Resources.LoadAll<Sprite>("Spritesheets/points");
        _spriteRenderer.sprite = _sprites[(int)Point];
		_position = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		_sinceStart += Time.fixedDeltaTime;
		transform.position = _position + new Vector2(0, _sinceStart / _duration * _height);
		if (_sinceStart > _duration) {
			Destroy(gameObject);
		}

	}
}
