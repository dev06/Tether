using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class NegativeImage : MonoBehaviour {

	private Image image; 
	private PlayerController player; 
	private bool started = false; 
	private bool started2 = true; 
	
	Color tint; 
	void Start () {
		image = GetComponent<Image>(); 
		player = PlayerController.Instance; 
		image.material.SetColor("_Color", new Color(0,0,0,0)); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		//started = !player.isHolding ? false : started; 

		
		

		if(player.isHolding && !started)
		{
			StopCoroutine("ShiftColor2"); 
			StopCoroutine("ShiftColor"); 
			StartCoroutine("ShiftColor");
			started2 = false;   
		}
		else if(!started2 && !player.isHolding)
		{
			StopCoroutine("ShiftColor"); 
			StopCoroutine("ShiftColor2"); 
			StartCoroutine("ShiftColor2"); 
			started = false; 
		}
	}


	float alpha = 0; 
	IEnumerator ShiftColor()
	{
		started = true; 
		alpha =image.material.GetColor("_Color").a; 
		while(alpha < 1f)
		{
			alpha+=Time.unscaledDeltaTime * 2.25f; 
			image.material.SetColor("_Color", new Color(alpha / 2f, alpha /2f, alpha /2f, alpha ));
			alpha= Mathf.Clamp(alpha, 0f, 1f); 
			yield return null; 
		}
	}

	IEnumerator ShiftColor2()
	{
		started2 = true; 
		//float alpha = image.material.GetColor("_Color").a /2f;  
		while(alpha > 0f)
		{
			alpha-=Time.unscaledDeltaTime * 7.25f; 
			image.material.SetColor("_Color", new Color(alpha /2f,  alpha /2f, alpha /2f, alpha)); 
			yield return null; 
		}
	}

}
