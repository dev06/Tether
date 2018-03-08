using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public delegate void BaseHit();
	public static BaseHit OnBaseHit;


	public delegate void Boost();
	public static Boost OnBoostStart;
	public static Boost OnBoostEnd;


	public delegate void Gameplay();
	public static Gameplay OnGameOver;
	public static Gameplay OnGameStart;
	public static Gameplay OnPause;
	public static Gameplay OnUnpause;
	public static Gameplay OnTutorialEnd;
	public static Gameplay OnTwirlActive;

	public delegate void HoldStatus(int i);
	public static HoldStatus OnHoldStatus;


	public delegate void GameState(State s);
	public static GameState OnStateChange;

	public delegate void LevelChange(Level changeLevel);
	public static LevelChange OnLevelChange;


	public delegate void ButtonPress(ButtonID id);
	public static ButtonPress OnButtonPress;


	public delegate void Mute(bool mute);
	public static Mute OnMute;

	public delegate void Reverb(bool active);
	public static Reverb OnReverb;

	public delegate void LockTaskComplete(LockTaskID id);
	public static LockTaskComplete OnLockTaskComplete;


	public delegate void Display(float x, float y);
	public static Display OnDisplayChange;
}
