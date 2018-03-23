using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {

    private float _spawnSpeed = 0.8f;
    private float _sinceSpawn = 0.0f;
    private Vector3 _spawnPosition;

	private float _flightHeight = 3.0f;
	
	// Use this for initialization
	void Start () {
        _spawnPosition = transform.position;
	}
	
	// Update is called once per frame
	private void FixedUpdate()
	{
		_sinceSpawn += Time.fixedDeltaTime;
		transform.position = _spawnPosition + new Vector3(0, Mathf.Sin(_sinceSpawn / _spawnSpeed * Mathf.PI) * _flightHeight, 0);
		if (_sinceSpawn > _spawnSpeed * 0.75) {
			ShowScore();
			Destroy(gameObject);
		}
	}

	private void ShowScore()
	{
        var prefab = Instantiate(Resources.Load("Prefabs/PointPrefab", typeof(GameObject))) as GameObject;
		prefab.GetComponent<PointScript>().Point = PointScript.Points.P400;
        prefab.transform.parent = GameObject.Find("Entities").transform;
        prefab.transform.localScale = new Vector3(1, 1, 1);
		prefab.transform.position = transform.position;
	}

}
