using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Track
{
	NONE,
	KILL,
	BELLS,
}

public class SwitchTrackHandler: MonoBehaviour {

	private float targetVolume;

	private AudioSource source;

	private AudioClip clip;

	private Track t;

	private float trackChangeRate = 2f;
	private float vel;
	void OnEnable()
	{
		EventManager.OnMute += OnMute;
	}
	void OnDisable()
	{
		EventManager.OnMute -= OnMute;
	}


	float master = 0;

	void OnMute(bool b)
	{
		if (GameplayController.LevelIndex == 1)
		{
			LockTaskPanel p = FindObjectOfType<LockTaskPanel>();

			if (p.Active)
			{
				return;
			}
		}


		// source.volume = b ? 0 : 1;

		StopCoroutine("Lerp");
		StartCoroutine("Lerp", b ? 0 : 1);
	}

	IEnumerator Lerp(float v)
	{
		while (source.volume != v)
		{
			source.volume = Mathf.SmoothDamp(source.volume, v, ref vel, Time.unscaledDeltaTime * 10f);
			yield return null;
		}
	}

	public void SetAudioSource(AudioSource s)
	{
		this.source = s;
	}



	public void SwitchTrack(Track t, AudioSource source, float volume)
	{

		StopCoroutine("Lerp");
		SetAudioSource(source);
		this.t = t;
		switch (t)
		{
			case Track.KILL:
			{
				clip = AppResources.Kill;
				break;
			}

			case Track.BELLS:
			{
				clip = AppResources.Bells;

				break;
			}
		}

		StopCoroutine("ISwitchTrack");
		StartCoroutine("ISwitchTrack", volume);
	}

	private IEnumerator ISwitchTrack(float t)
	{
		while (source.volume > 0)
		{
			source.volume -= Time.unscaledDeltaTime * trackChangeRate;
			yield return null;
		}

		source.Stop();

		source.clip = clip;

		source.Play();

		source.time = Random.Range(10, clip.length - 120f);


		while (source.volume < t)
		{
			source.volume += Time.unscaledDeltaTime * trackChangeRate;
			yield return null;
		}
	}
}
