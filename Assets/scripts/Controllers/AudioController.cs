using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioController : MonoBehaviour {


	public static AudioController Instance;

	public static bool Mute;

	private float menu_pitch = .85f;

	private float slomo_pitch = .6f;

	private float default_pitch = 1f;

	private Transform targetTransform;

	private Mixer mixer;

	private PlayerController player;

	private AudioSource source;

	private float slowmo_freq = 500f;

	private float default_freq = 1500;

	private SwitchTrackHandler trackHandler;

	private Track currentTrack;


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
		EventManager.OnLevelChange += OnLevelChange;
		EventManager.OnMute += OnMute;

	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnHoldStatus -= OnHoldStatus;
		EventManager.OnLevelChange -= OnLevelChange;
		EventManager.OnMute -= OnMute;
	}

	public void Init()
	{
		player = PlayerController.Instance;

		trackHandler = GetComponent<SwitchTrackHandler>();

		source = GetComponent<AudioSource>();

		targetTransform = Camera.main.transform;

		mixer = GetComponent<Mixer>();

		StartCoroutine("SetMixer", slowmo_freq);
		StartCoroutine("SetPitch", menu_pitch);
	}

	void OnGameStart()
	{
		StopAllCoroutines();
		StartCoroutine("SetPitch", default_pitch);
		StartCoroutine("SetMixer", default_freq);
		//mixer.SetFloat("Lowpass", 5000f);
	}

	void OnGameOver()
	{
		StopAllCoroutines();
		StartCoroutine("SetMixer", slowmo_freq);
		StartCoroutine("SetPitch", menu_pitch);
		//mixer.SetFloat("Lowpass", slowmo_freq);
		SwitchTrack(Level.LEVEL1);
	}

	void Update ()
	{
		//	Debug.Log(Mute);
	}

	void FixedUpdate()
	{
		transform.position = targetTransform.position;
	}

	void OnLevelChange(Level l)
	{
		SwitchTrack(l);
	}

	private void SwitchTrack(Level l)
	{
		Track t = Track.NONE;
		switch (l)
		{
			case Level.LEVEL1:
			{
				t = Track.KILL;
				break;
			}
			case Level.LEVEL2:
			{
				t = Track.BELLS;
				break;
			}
		}

		trackHandler.SwitchTrack(t, source);
		SetCurrentTrack(t);
	}

	void OnMute(bool b)
	{
		StopCoroutine("SetVolume");
		StartCoroutine("SetVolume", b ? 0f : 1f);
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

	IEnumerator SetVolume(float v)
	{
		float vVel = 0;
		float current;
		while (source.volume != v)
		{
			current = Mathf.SmoothDamp(source.volume, v, ref vVel, Time.unscaledDeltaTime * 10f);
			source.volume = current;
			yield return null;
		}
	}

	void OnHoldStatus(int i)
	{
		if (i == 1)
		{
			StopCoroutine("SetMixer");
			StopCoroutine("SetPitch");

			StartCoroutine("SetPitch", slomo_pitch);
			StartCoroutine("SetMixer", slowmo_freq);
		}
		else
		{
			StopCoroutine("SetMixer");
			StopCoroutine("SetPitch");
			StartCoroutine("SetPitch", default_pitch);
			StartCoroutine("SetMixer", default_freq);
		}
	}

	public void SetCurrentTrack(Track t)
	{
		this.currentTrack = t;
	}


	public string CurrentTrack()
	{
		switch (currentTrack)
		{
			case Track.KILL:
				return "Kill (Reborn) - Sauniks";
			case Track.BELLS:
				return "Carols Of The Bells - Sauniks";
		}

		return "Kill (Reborn) - Sauniks";
	}

	public void Play()
	{
		source.Play();
	}
}
