using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class SettingIcon : MonoBehaviour {

	public Sprite backArrow; 
	private Sprite gear; 
	private Image image; 
	private bool active; 
	void Start () 
	{
		image = GetComponent<Image>(); 	
		gear = image.sprite; 
	}
	
	void FixedUpdate () 
	{
		if(active) return; 
		transform.Rotate(new Vector3(0, 0, -45f * Time.unscaledDeltaTime)); 
	}

	public void SetActive(bool b)
	{
		this.active = b; 

		if(active)
		{
			image.sprite = backArrow; 
			transform.rotation = Quaternion.Euler(Vector3.zero); 
		}
		else
		{
			image.sprite = gear; 
		}
	}
}
