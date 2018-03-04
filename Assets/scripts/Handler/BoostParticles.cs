using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostParticles : MonoBehaviour {

	public Particle boostComplete;
	void Start ()
	{

	}

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
		boostComplete.Play();
	}

	IEnumerator Play()
	{
		yield return new WaitForSeconds(1);

	}

	// Update is called once per frame
	void Update () {

	}
}
