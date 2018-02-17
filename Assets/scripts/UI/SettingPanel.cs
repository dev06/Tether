using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour {


	Animation animation;

	public bool Active; 

	private SettingIcon settingIcon; 

	void OnEnable()
	{
		EventManager.OnButtonPress+=OnButtonPress; 
	}

	void OnDisable()
	{
		EventManager.OnButtonPress-=OnButtonPress; 
	}

	void OnButtonPress(ButtonID id)
	{
		if(id != ButtonID.SETTINGS) 
		{
			return;
		} 
		Animate(); 
	}



	void Start () 
	{
		animation = GetComponent<Animation>(); 

		settingIcon = transform.GetChild(0).GetComponent<SettingIcon>(); 
	}

	void Update () {

	}

	public void Animate()
	{
		Active = !Active; 
		if(!Active)
		{
			animation[animation.clip.name].speed = -1;
			animation [animation.clip.name].time = animation[animation.clip.name].length;
			animation.Play(); 		
			Active = false; 
		}
		else
		{
			animation[animation.clip.name].speed = 1;
			animation [animation.clip.name].time = 0;
			animation.Play(); 
			Active = true; 

		}

		settingIcon.SetActive(Active); 
	}
}
