using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticles : MonoBehaviour {

	public ParticleSystem boostParticles;

	void OnEnable()
	{
		EventManager.OnTwirlActive += OnTwirlActive;
	}

	void OnDisable()
	{
		EventManager.OnTwirlActive -= OnTwirlActive;
	}

	void OnTwirlActive()
	{
		boostParticles.Play();
	}
}
