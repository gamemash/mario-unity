using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlockScript : MonoBehaviour
{

	public enum SpawnOption
	{
		Coin,
		Mushroom,
		Star
	}

	public enum State
	{
		Active,
		Spent
	}

	public SpawnOption Spawn = SpawnOption.Coin;

	private State _state = State.Active;
	private float _timeSinceBump = 1.0f;
	private float _bumpTime = 0.3f;
	private float _bumpHeight = 0.5f;


	private Vector2 _position;

	// Use this for initialization
	void Start ()
	{
		_position = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (_timeSinceBump < _bumpTime) {
			transform.position = _position + new Vector2(0, Mathf.Sin(_timeSinceBump * Mathf.PI / _bumpTime) * _bumpHeight);
			_timeSinceBump += Time.fixedDeltaTime;
			if (_timeSinceBump > _bumpTime)
				_afterBump();
		}
	}

	public void Bump()
	{
		if (_state == State.Active) {
			_timeSinceBump = 0.0f;
			GetComponent<Animator>().SetTrigger("bump");
			_state = State.Spent;
		}
	}

	private void _afterBump()
	{
		switch (Spawn) {
			case SpawnOption.Mushroom:
				var mushroomPrefab = Instantiate(Resources.Load("Prefabs/MushroomPrefab", typeof(GameObject))) as GameObject;
				mushroomPrefab.transform.parent = GameObject.Find("Entities").transform;
				mushroomPrefab.transform.localScale = new Vector3(1, 1, 1);
				mushroomPrefab.transform.position = (Vector3)_position + new Vector3(0, 0, 1);
				mushroomPrefab.GetComponent<MushroomScript>().Spawn();
				break;
			
			case SpawnOption.Coin:
				var prefab = Instantiate(Resources.Load("Prefabs/CoinPrefab", typeof(GameObject))) as GameObject;
				prefab.transform.parent = GameObject.Find("Entities").transform;
				prefab.transform.localScale = new Vector3(1, 1, 1);
				prefab.transform.position = (Vector3)_position + new Vector3(0, 0, 1);
				
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Player")) {
            var yComponent = (other.contacts[0].point - (Vector2) transform.position).y;
			if (yComponent < -0.4) {
				Bump();
			}
		}
	}
}
