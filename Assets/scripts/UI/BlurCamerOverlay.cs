using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlurCamerOverlay : MonoBehaviour {

	public GameObject RemoveLevel2GradObj;


	void OnEnable()
	{
		EventManager.OnGameStart += OnGameStart;
	}

	void OnDisable()
	{
		EventManager.OnGameStart -= OnGameStart;
	}

	void OnGameStart()
	{
		Debug.Log("Cal;le");
		RemoveLevel2GradObj.SetActive(false);
	}


	void Start ()
	{

	}


	void Update ()
	{

	}
}
