using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 
public class AudioController : MonoBehaviour {


	public static AudioController Instance; 

	private Mixer mixer; 

	private PlayerController player; 

	private AudioSource source; 


	void Awake()
	{
		if(Instance == null)
		{
			Instance = this; 
		}
		else
		{
			DestroyImmediate(gameObject); 
		}
	}

	void OnEnable()
	{
		EventManager.OnGameStart+=OnGameStart; 
		EventManager.OnGameOver+=OnGameOver; 
		EventManager.OnHoldStatus+=OnHoldStatus; 

	}

	void OnDisable()
	{
		EventManager.OnGameStart-=OnGameStart; 
		EventManager.OnGameOver-=OnGameOver; 
		EventManager.OnHoldStatus-=OnHoldStatus; 
	}

	void Start () 
	{
		Init(); 
	}
	
	void Init()
	{
		player = PlayerController.Instance; 

		source = GetComponent<AudioSource>(); 

		mixer = GetComponent<Mixer>(); 

		source.time = Random.Range(1f, 3f); 

	}

	void OnGameStart()
	{
		
		StartCoroutine("SetMixer", 5000f); 
		//mixer.SetFloat("Lowpass", 5000f); 
	}

	void OnGameOver()
	{
		//StartCoroutine("SetMixer", 200f); 
		mixer.SetFloat("Lowpass", 200f); 
	}

	void Update () 
	{
	}

	IEnumerator SetMixer(float v)
	{
		float mixerVel = 0; 
		float current ; 
		float vv =0 ;

		while(true)
		{
			mixer.GetFloat("Lowpass", out vv); 
			current = Mathf.SmoothDamp(vv , v, ref mixerVel, Time.deltaTime * 15f); 
			mixer.SetFloat("Lowpass", current); 
			yield return null; 
		}
	}

	void OnHoldStatus(int i)
	{
		if(i == 1)
		{
			StopCoroutine("SetMixer"); 
			StartCoroutine("SetMixer", 100f); 
			//mixer.SetFloat("Lowpass", 50f); 
		}
		else
		{
			StopCoroutine("SetMixer"); 
			StartCoroutine("SetMixer", 5000f); 
		}
	}
}
