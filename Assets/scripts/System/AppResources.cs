using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppResources : MonoBehaviour {

	public static GameObject Coin = (GameObject)Resources.Load("prefabs/Coin") as GameObject;
	public static GameObject CoinGroup = (GameObject)Resources.Load("prefabs/CoinGroup") as GameObject;



	public static GameObject Base = (GameObject)Resources.Load("prefabs/Base") as GameObject;

	public static GameObject Effect = (GameObject)Resources.Load("prefabs/Effect") as GameObject;
	public static GameObject Boom = (GameObject)Resources.Load("prefabs/Boom") as GameObject;



	// public static AudioClip IntoTheDarkness = (AudioClip)Resources.Load("audio/tracks/theme") as AudioClip;

	public static AudioClip Kill = (AudioClip)Resources.Load("audio/tracks/Kill") as AudioClip;
	public static AudioClip Bells = (AudioClip)Resources.Load("audio/tracks/Bells") as AudioClip;

}
