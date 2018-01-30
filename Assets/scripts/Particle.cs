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
		if(particle == null)
		{
			particle = GetComponent<ParticleSystem>(); 
		}
		particle.Play();
	}

	public void Play(Vector3 position)
	{
		if(particle == null)
		{
			particle = GetComponent<ParticleSystem>(); 
		}
		SetPosition(position);
		particle.Play();
	}

	public void Play(Vector3 position, Color c)
	{
		if(particle == null)
		{
			particle = GetComponent<ParticleSystem>(); 
		}
		particle.startColor = c;
		SetPosition(position);
		particle.Play();
	}

	public void Stop()
	{
		if(particle == null)
		{
			particle = GetComponent<ParticleSystem>(); 
		}
		particle.Stop();
	}
	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}
}
