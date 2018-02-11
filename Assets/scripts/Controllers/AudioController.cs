using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioController : MonoBehaviour {


	public static AudioController Instance;

	public float slowmot_pitch = .85f; 

	private Mixer mixer;

	private PlayerController player;

	private AudioSource source;

	private float slowmo_freq = 150f;

	private float default_freq = 1500f;


	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if (Instance == null)
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
		EventManager.OnGameStart += OnGameStart;
		EventManager.OnGameOver += OnGameOver;
		EventManager.OnHoldStatus += OnHoldStatus;

	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnHoldStatus -= OnHoldStatus;
	}

	public void Init()
	{
		player = PlayerController.Instance;

		source = GetComponent<AudioSource>();

		mixer = GetComponent<Mixer>();

		mixer.SetFloat("Lowpass", slowmo_freq);

		StartCoroutine("SetPitch", slowmot_pitch);
	}

	void OnGameStart()
	{
		StopAllCoroutines();
		StartCoroutine("SetPitch", 1f);
		StartCoroutine("SetMixer", default_freq);
		//mixer.SetFloat("Lowpass", 5000f);
	}

	void OnGameOver()
	{
		StopAllCoroutines();
		StartCoroutine("SetMixer", slowmo_freq);
		StartCoroutine("SetPitch", slowmot_pitch);
		//mixer.SetFloat("Lowpass", slowmo_freq);
	}

	void Update ()
	{

	}

	IEnumerator SetMixer(float v)
	{
		float mixerVel = 0;
		float current ;
		float vv = 0 ;

		while (true)
		{
			mixer.GetFloat("Lowpass", out vv);
			current = Mathf.SmoothDamp(vv , v, ref mixerVel, Time.deltaTime * 15f);
			mixer.SetFloat("Lowpass", current);
			yield return null;
		}
	}

	IEnumerator SetPitch(float p)
	{
		float pitchVel = 0;
		float current;
		while (true)
		{
			current = Mathf.SmoothDamp(source.pitch, p, ref pitchVel, Time.deltaTime * 10f);
			source.pitch = current;
			yield return null;
		}
	}

	void OnHoldStatus(int i)
	{
		if (i == 1)
		{
			StopCoroutine("SetMixer");
			StartCoroutine("SetMixer", slowmo_freq);
		}
		else
		{
			StopCoroutine("SetMixer");
			StartCoroutine("SetMixer", default_freq);
		}
	}
}
