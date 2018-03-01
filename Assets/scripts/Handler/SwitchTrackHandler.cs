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

	private AudioSource source;

	private AudioClip clip;

	private Track t;

	private float trackChangeRate = 2f;
	public void SwitchTrack(Track t, AudioSource source, float volume)
	{
		float targetVolume = AudioController.Mute ? 0f : volume;

		this.source = source;
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
		StartCoroutine("ISwitchTrack", targetVolume);
	}

	private IEnumerator ISwitchTrack(float targetVolume)
	{
		while (source.volume > 0)
		{
			source.volume -= Time.unscaledDeltaTime * trackChangeRate;
			yield return null;
		}

		source.Stop();

		source.clip = clip;

		source.Play();

		source.time = Random.Range(10, clip.length - 60f);

		while (source.volume < targetVolume)
		{
			source.volume += Time.unscaledDeltaTime * trackChangeRate;
			yield return null;
		}
	}
}
