using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapticPlugin;
public class Haptic : MonoBehaviour {


	public static void Vibrate(HapticIntensity intensity)
	{
#if UNITY_IPHONE && !UNITY_EDITOR
		switch (intensity)
		{
			case HapticIntensity.Light:
			{
				TapticManager.Vibrate(ImpactFeedback.Light);
				break;
			}
			case HapticIntensity.Medium:
			{
				TapticManager.Vibrate(ImpactFeedback.Midium);
				break;
			}
			case HapticIntensity.Heavy:
			{
				TapticManager.Vibrate(ImpactFeedback.Heavy);
				break;
			}
		}
#else

		switch (intensity)
		{
			case HapticIntensity.Light:
			{
				Vibration.Vibrate(3);
				break;
			}
			case HapticIntensity.Medium:
			{
				Vibration.Vibrate(5);
				break;
			}
			case HapticIntensity.Heavy:
			{
				Vibration.Vibrate(7);
				break;
			}
		}
#endif
	}


}
public enum HapticIntensity
{
	Light,
	Medium,
	Heavy,
}
