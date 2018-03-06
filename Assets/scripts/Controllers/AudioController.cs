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

	private float targetPitch;

	private float targetVol;

	private float targetMixer;

	private float v1, v2, v3;

	private float mv;

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

		if (init == false)
		{
			source.time = Random.Range(0f, source.clip.length - 120f);
		}

		init = true;

		ToggleReverb(AudioController.ReverbOn);


		targetVol = default_vol;

		targetPitch = menu_pitch;

		targetMixer = slowmo_freq;

	}


	void OnGameStart()
	{

		targetPitch = default_pitch;

		targetMixer = default_freq;
	}

	void OnGameOver()
	{
		SwitchTrack(Level.LEVEL1);
	}

	void OnPause()
	{
		targetPitch = pause_pitch;

		targetMixer = pause_freq;
	}

	void OnUnpause()
	{
		targetPitch = default_pitch;

		targetMixer = default_freq;
	}



	void Update ()
	{

		source.pitch = Mathf.SmoothDamp(source.pitch, targetPitch, ref v1, Time.deltaTime * 10f);

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

	public void SwitchTrack(Level l)
	{

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
	}

	void OnHoldStatus(int i)
	{
		targetPitch = (i == 1) ? slomo_pitch : default_pitch;

		targetMixer = (i == 1) ? slowmo_freq : default_freq;
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
				name = !p.Active ?  "Carol Of The Bells - Sauniks" : "Locked";
				break;
			}
		}

		return name;
	}

	public void Play()
	{
		//SwitchTrack(Level.LEVEL1);
		//source.Play();
	}
}
