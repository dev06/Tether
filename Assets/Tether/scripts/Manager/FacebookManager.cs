using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity.Mobile;
public class FacebookManager : MonoBehaviour {

	public static FacebookManager instance; 
	void Awake()
	{
		DontDestroyOnLoad(gameObject); 
		if(instance == null)
		{
			instance = this; 
		}
		else
		{
			Destroy(gameObject); 
		}
#if !UNITY_EDITOR
		if (!Facebook.Unity.FB.IsInitialized)
		{
			Facebook.Unity.FB.Init();
		} 
		else
		{
			Facebook.Unity.FB.ActivateApp();
		}
#endif
	}
}
