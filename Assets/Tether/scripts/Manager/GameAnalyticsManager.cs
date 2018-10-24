using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tether
{
	public class GameAnalyticsManager : MonoBehaviour {

		void Awake()
		{
			DontDestroyOnLoad(gameObject); 
		}

	}

}
