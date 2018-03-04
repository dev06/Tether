using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioController : MonoBehaviour {


	public static AudioController Instance;

	public bool playOnAwake;

	public static bool Mute;

	public static bool ReverbOn;

	private  float menu_pitch = .85f;

	private  float slomo_pitch = .6f;

	private  float pause_pitch = .3f;

	private  float pause_vol = .1f;

	private  float default_vol = 1f;

	private  float default_pitch = 1f;

	private Transform targetTransform;

	private Mixer mixer;

	private PlayerController player;

	private AudioSource source;

	private float slowmo_freq = 500f;

	private float default_freq = 1500;

	private float pause_freq = 100;

	private AudioReverbZone reverbZone;

	private bool init;



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
		EventManager.OnPause += OnPause;
		EventManager.OnUnpause += OnUnpause;
		EventManager.OnStateChange += OnStateChange;

	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
		EventManager.OnGameOver -= OnGameOver;
		EventManager.OnHoldStatus -= OnHoldStatus;
		EventManager.OnLevelChange -= OnLevelChange;
		EventManager.OnMute -= OnMute;
		EventManager.OnPause -= OnPause;
		EventManager.OnUnpause -= OnUnpause;
		EventManager.OnStateChange -= OnStateChange;
	}

	public void Init()
	{


		player = PlayerController.Instance;

		trackHandler = GetComponent<SwitchTrackHandler>();

		reverbZone = GetComponent<AudioReverbZone>();

		source = GetComponent<AudioSource>();

		trackHandler.SetAudioSource(source);

		targetTransform = Camera.main.transform;

		mixer = GetComponent<Mixer>();

		// if(PlayerPrefs.HasKey("ReverbToggle"))
		// {
		// }

		if (init == false)
		{
			source.time = Random.Range(0f, source.clip.length - 120f);
		}

		init = true;

		ToggleReverb(AudioController.ReverbOn);

		targetVol = default_vol;
		targetPitch = menu_pitch;
		targetMixer = slowmo_freq;

		// StopAllCoroutines();
		// Debug.LogError("Init called");
		// targetPitch = menu_pitch;
		// StartCoroutine("SetMixer", slowmo_freq);
		// //StartCoroutine("SetPitch", menu_pitch);
		// StartCoroutine("SetVolume", default_vol);
	}



	void OnStateChange(State s)
	{

		try
		{

			// if(s == State.MENU)
			// {
			// 	StopAllCoroutines();
			// 	if(!init)
			// 	{
			// 		Init();
			// 	}
			// 	StartCoroutine("SetMixer", slowmo_freq);
			// 	StartCoroutine("SetPitch", menu_pitch);
			// 	StartCoroutine("SetVolume", default_vol);
			// }
		}
		catch (System.Exception e)
		{
			Debug.Log("Wrong");
		}

	}

	void OnGameStart()
	{
		// StopAllCoroutines();
		// StartCoroutine("SetPitch", default_pitch);
		// StartCoroutine("SetMixer", default_freq);
		//mixer.SetFloat("Lowpass", 5000f);

		targetPitch = default_pitch;
		targetMixer = default_freq;
	}

	void OnGameOver()
	{
//		StopAllCoroutines();
		SwitchTrack(Level.LEVEL1);
		// StartCoroutine("SetMixer", slowmo_freq);
		// StartCoroutine("SetPitch", menu_pitch);
	}

	void OnPause()
	{
		//		StopAllCoroutines();
		// StartCoroutine("SetMixer", pause_freq);
		// StartCoroutine("SetPitch", pause_pitch);
		// StartCoroutine("SetVolume", pause_vol);

		targetPitch = pause_pitch;
		targetMixer = pause_freq;
	}

	void OnUnpause()
	{
		// StopAllCoroutines();
		// StartCoroutine("SetPitch", default_pitch);
		// StartCoroutine("SetMixer", default_freq);
		// StartCoroutine("SetVolume", default_vol);

		targetPitch = default_pitch;
		targetMixer = default_freq;
	}

	float targetPitch;
	float targetVol;
	float targetMixer;
	float v1, v2, v3;
	float mv;
	void Update ()
	{
		source.pitch = Mathf.SmoothDamp(source.pitch, targetPitch, ref v1, Time.deltaTime * 10f);
		//source.volume = Mathf.SmoothDamp(source.volume, targetVol, ref v2, Time.deltaTime * 10f);
		mixer.GetFloat("Lowpass", out mv);
		mixer.SetFloat("Lowpass", Mathf.SmoothDamp(mv, targetMixer, ref v3, Time.deltaTime * 10f));

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
		//StopAllCoroutines();
		Track t = Track.NONE;
		float volume = 0;
		if (!playOnAwake) { return; }

		switch (l)
		{
			case Level.LEVEL1:
			{
				t = Track.KILL;
				volume = 1;
				break;
			}
			case Level.LEVEL2:
			{
				t = Track.BELLS;
				LockTaskPanel p = FindObjectOfType<LockTaskPanel>();
				volume = !p.Active ? 1f : 0f;
				break;
			}
		}

		volume = Mute ? 0f : volume;

		trackHandler.SwitchTrack(t, source, volume);
		SetCurrentTrack(t);
	}

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

		targetVol = b ? 0f : default_vol;
		//StopCoroutine("SetVolume");
		//	StartCoroutine("SetVolume", b ? 0f : default_vol);
	}

	// IEnumerator SetMixer(float v)
	// {
	// 	//Debug.LogError("Mixer Start");
	// 	float mixerVel = 0;
	// 	float current ;
	// 	float vv = 0 ;

	// 	while (true)
	// 	{
	// 		mixer.GetFloat("Lowpass", out vv);
	// 		current = Mathf.SmoothDamp(vv , v, ref mixerVel, Time.deltaTime * 15f);
	// 		mixer.SetFloat("Lowpass", current);
	// 		yield return null;
	// 	}
	// }

	// IEnumerator SetPitch(float p)
	// {

	// 	float pitchVel = 0;
	// 	float current;
	// 	while (true)
	// 	{
	// 		current = Mathf.SmoothDamp(source.pitch, p, ref pitchVel, Time.deltaTime * 10f);
	// 		source.pitch = current;
	// 		Debug.LogError(source.pitch);
	// 		yield return null;
	// 	}
	// }

	// IEnumerator SetVolume(float v)
	// {
	// 	float vVel = 0;
	// 	float current;
	// 	while (source.volume != v)
	// 	{
	// 		current = Mathf.SmoothDamp(source.volume, v, ref vVel, Time.unscaledDeltaTime * 10f);
	// 		source.volume = current;
	// 		yield return null;
	// 	}
	// }

	void OnHoldStatus(int i)
	{
		if (i == 1)
		{
			// StopCoroutine("SetMixer");
			// StopCoroutine("SetPitch");

			// StartCoroutine("SetPitch", slomo_pitch);
			// StartCoroutine("SetMixer", slowmo_freq);

			targetPitch = slomo_pitch;
			targetMixer = slowmo_freq;
		}
		else
		{
			// StopCoroutine("SetMixer");
			// StopCoroutine("SetPitch");
			// StartCoroutine("SetPitch", default_pitch);
			// StartCoroutine("SetMixer", default_freq);

			targetPitch = default_pitch;
			targetMixer = default_freq;
		}
	}

	public void SetCurrentTrack(Track t)
	{
		this.currentTrack = t;
	}

	public void ToggleReverb(bool b)
	{
		if (reverbZone == null)
		{
			reverbZone = GetComponent<AudioReverbZone>();
		}

		reverbZone.enabled = b;
		ReverbOn = b;
	}


	public string CurrentTrack()
	{
		string name = "Kill (Reborn) - Sauniks";
		switch (currentTrack)
		{
			case Track.KILL:
			{
				name =  "Kill (Reborn) - Sauniks";
				break;
			}
			case Track.BELLS:
			{
				LockTaskPanel p = FindObjectOfType<LockTaskPanel>();
				name = !p.Active ?  "Carols Of The Bells - Sauniks" : "Locked";
				break;
			}
		}

		return name;
	}

	public void Play()
	{
		source.Play();
	}
}
