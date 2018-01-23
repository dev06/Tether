using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	ParticleSystem particle;
	// Use this for initialization
	void Start () {
		particle = GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update () {

	}


	public void Play()
	{
		if (particle == null) return;
		if (particle.isPlaying) return;
		particle.Play();
	}

	public void Play(Vector3 position)
	{
		SetPosition(position);
		particle.Play();
	}

	public void Play(Vector3 position, Color c)
	{
		particle.startColor = c;
		SetPosition(position);
		particle.Play();
	}

	public void Stop()
	{
		particle.Stop();
	}
	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}
}
