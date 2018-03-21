using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CaptionHandler : MonoBehaviour {

	Text text;
	string message = "- Made with pure passion...";


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
		if (s == State.MENU)
		{
			StopCoroutine("IType");
			StartCoroutine("IType");
		}
	}

	void Start ()
	{
		text = GetComponent<Text>();
	}

	IEnumerator IType()
	{
		yield return new WaitForSeconds(1f);
		string msg = "";
		text.text = "";
		for (int i = 0; i < message.Length; i++)
		{
			msg += message[i];
			text.text = msg;
			yield return new WaitForSeconds(.15f);
		}
	}
}
