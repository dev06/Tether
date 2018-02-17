using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : ParentUI {


	void OnEnable()
	{
		EventManager.OnPause+=OnPause; 
		EventManager.OnUnpause+=OnUnpause; 
	}

	void OnDisable()
	{
		EventManager.OnPause-=OnPause; 
		EventManager.OnUnpause-=OnUnpause; 

	}



	void Start ()
	{
		Init();
	}

	void OnPause()
	{
		Show(); 
	}

	void OnUnpause()
	{
		Hide(); 
	}

}
