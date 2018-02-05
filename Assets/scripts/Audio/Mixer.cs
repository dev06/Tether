using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class Mixer : MonoBehaviour {


	public AudioMixer mixer; 


	public void SetFloat(string name, float value)
	{
		if(mixer == null)
		{
			Debug.LogError("Mixer is null " + gameObject.name); 
			return; 
		}

		mixer.SetFloat(name, value); 
	}

	public bool GetFloat(string name, out float f)
	{
		if(mixer == null)
		{
			Debug.LogError("Mixer is null " + gameObject.name); 
			f = 0; 
			return false; 
		}

		return mixer.GetFloat(name, out f); 
	}
}
