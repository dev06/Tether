using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	SpriteRenderer renderer;
	BoxCollider2D collider;
	ParticleSystem effect;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
		effect = GetComponentInChildren<ParticleSystem>();
	}

	void OnEnable()
	{
		EventManager.OnBoostStart += OnBoostStart;
		EventManager.OnBoostEnd += OnBoostEnd;
	}

	void OnDisable()
	{
		EventManager.OnBoostStart -= OnBoostStart;
		EventManager.OnBoostEnd -= OnBoostEnd;
	}

	void OnBoostStart()
	{
		effect.transform.gameObject.SetActive(false);
		effect.Stop();
	}

	void OnBoostEnd()
	{
		if (IsInView())
		{
			effect.transform.gameObject.SetActive(true);
		}
	}


	void Update ()
	{
		renderer.enabled = IsInView();
		collider.enabled = renderer.enabled;

		if (IsInView())
		{
			if (effect.isPlaying == false)
			{
				effect.transform.gameObject.SetActive(true);
				effect.Play();
			}
		}
		else
		{
			if (effect.isPlaying)
			{
				effect.Stop();
				effect.transform.gameObject.SetActive(false);
			}
		}
	}

	bool IsInView()
	{
		Vector3 port = Camera.main.WorldToViewportPoint(transform.position);

		if (port.y > 0 && port.y < 1)
		{
			return true;
		}

		return false;
	}
}
