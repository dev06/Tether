﻿using System.Collections;
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


	public delegate void GameState(State s);
	public static GameState OnStateChange; 
}
