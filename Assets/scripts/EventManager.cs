using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public delegate void BaseHit(BaseController hitBase, Vector2 hitPoint);
	public static BaseHit OnBaseHit;
}
