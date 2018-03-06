using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : ParentUI {

	private Animation animation;
	private bool done;
	void Start ()
	{
		Init();
		if (GameplayController.GAME_STATE == State.INTRO)
		{
			Show();
		}
		animation = GetComponent<Animation>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (GameplayController.GAME_STATE != State.INTRO) { return; }

		if (animation.isPlaying == false)
		{
			if (!done)
			{

				GameplayController.SetState(State.MENU);

				//GameplayController.GAME_STATE = State.MENU;
				AudioController.Instance.SwitchTrack(Level.LEVEL1);

				Hide();
				done = true;
			}
		}
	}
}
