using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Level
{
	LEVEL1,
	LEVEL2,
}

public class Constants
{
	public static Color Level1_CameraBackground = new Color(34f / 255f, 34f / 255f, 34f / 255f, 1f);
	public static Color Level1_Accent = Color.white;
	public static Color Level1_Sauniks = new Color(1f, 1f, 1f, 10f / 255f);

	public static Color Level2_CameraBackground = new Color(210f / 255f, 210f / 255f, 210f / 255f, 1f);
	public static Color Level2_Accent = Color.black;
	public static Color Level2_Sauniks = Color.black;

}


public class LevelController : MonoBehaviour {

	public static LevelController Instance;

	private bool isInit;

	public Level level;

	private Camera camera;

	private Transform bases;

	private GameObject[] border;

	private Transform powerup;

	private GameObject[] particles;

	private ScoreHandler scoreHandler;


	private PlayerController playerController;


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	public void Init()
	{
		camera = Camera.main;
		bases = GameObject.FindWithTag("Objects/bases").transform;
		border = GameObject.FindGameObjectsWithTag("Objects/border");
		powerup = GameObject.FindWithTag("Objects/powerup").transform;
		scoreHandler = FindObjectOfType<ScoreHandler>();
		particles = GameObject.FindGameObjectsWithTag("Particles/slowmo_1");
		playerController = PlayerController.Instance;
		isInit = true;
	}




	public void SetLevel(Level l)
	{
		if (!isInit)
		{
			Init();
		}

		this.level = l;

		switch (l)
		{
			case Level.LEVEL1:
			{
				Color[] palette = { Constants.Level1_CameraBackground, Constants.Level1_Accent, Constants.Level1_Sauniks};
				SwitchColorPallette(palette);
				break;
			}
			case Level.LEVEL2:
			{
				Color[] palette = {Constants.Level2_CameraBackground, Constants.Level2_Accent, Constants.Level2_Sauniks};

				SwitchColorPallette(palette);
				break;
			}
		}


		if (EventManager.OnLevelChange != null)
		{
			EventManager.OnLevelChange(this.level);
		}

		GameplayController.Level = this.level;
	}

	private void SwitchColorPallette(Color[] palette)
	{
		int p_cam = 0;
		int p_acc = 1;
		int p_sau = 2;

		camera.backgroundColor = palette[p_cam];



		playerController.SetColor(palette[p_acc]);

		scoreHandler.scoreText.color = palette[p_acc];


		for (int i = 0; i < bases.childCount; i++)
		{
			SpriteRenderer sr = bases.GetChild(i).GetComponent<SpriteRenderer>();

			sr.color = palette[p_acc];
		}

		for (int i = 0; i < border.Length; i++)
		{
			SpriteRenderer sr = border[i].GetComponent<SpriteRenderer>();

			sr.color = palette[p_acc];
		}

		for (int i = 0; i < powerup.childCount; i++)
		{
			SpriteRenderer sr = powerup.GetChild(i).GetComponent<SpriteRenderer>();

			sr.color = palette[p_acc];
		}

		for (int i = 0 ; i < particles.Length; i++)
		{
			Particle p = particles[i].GetComponent<Particle>();

			p.SetStartColor(palette[p_acc]);
		}
	}

	// void OnValidate()
	// {
	// 	try
	// 	{
	// 		Init();
	// 		if(level == Level.LEVEL1)
	// 		{
	// 			Color[] palette = { Constants.Level1_CameraBackground, Constants.Level1_Accent,Constants.Level1_Sauniks};
	// 			SwitchColorPallette(palette)	;
	// 		}
	// 		else
	// 		{
	// 			Color[] palette = {Constants.Level2_CameraBackground, Constants.Level2_Accent, Constants.Level2_Sauniks};
	// 			SwitchColorPallette(palette);

	// 		}
	// 	}
	// 	catch(System.Exception e)
	// 	{

	// 	}

	// }

}
