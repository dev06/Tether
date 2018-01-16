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

		particle.Play();
	}

	public void SetPosition(Vector3 position)
	{
		transform.position = position;
	}
}
