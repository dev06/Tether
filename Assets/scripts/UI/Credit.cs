using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : ParentUI 
{

	private Animation animation; 

	void Start ()
	{
		Init();
		Hide(); 

		animation = GetComponent<Animation>(); 
	}

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
	}


	void OnStateChange(State s)
	{
		if(s != State.CREDIT) {
			Hide(); 
			return;
		} 
		animation.Play(); 
		Show(); 
	}
}
