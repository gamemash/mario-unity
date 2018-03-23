using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelControllerScript : MonoBehaviour
{
	private AudioClip _music;
	private AudioSource _audioSource;


	public GameObject TimeLeftObject;

	private float _timeLeft;

	// Use this for initialization
	void Start ()
	{
		_music = Resources.Load<AudioClip>("Audio/smb_ground_theme");
		_audioSource = GetComponent<AudioSource>();
		_audioSource.clip = _music;
		_audioSource.Play();
		_timeLeft = 380;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timeLeft -= Time.deltaTime;
		TimeLeftObject.GetComponent<Text>().text = String.Format("{0}",(int)_timeLeft);


	}
}
