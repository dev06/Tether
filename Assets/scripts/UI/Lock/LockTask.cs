using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LockTaskID
{
	ID_1,
	ID_2,
	ID_3,
}

public class LockTaskValue
{
	public static int Task1Value = 1;
	public static int Task2Value = 1;
	public static int Task3Value = 1;
}
public class LockTask : MonoBehaviour {

	public LockTaskID taskID;
	private Image background;
	private Text taskText;
	public Color unlockedColor = Color.green;
	public Color lockedColor = Color.red;
	public bool completed;

	void OnEnable()

	{
		EventManager.OnLockTaskComplete += OnLockTaskComplete;
	}
	void Disable()
	{
		EventManager.OnLockTaskComplete -= OnLockTaskComplete;
	}


	void OnLockTaskComplete(LockTaskID id)
	{
		if (this.taskID == id)
		{
			SetCompleted(true);
			PlayerPrefs.SetString("LockTask" + id, completed.ToString());
		}
	}

	void Start ()
	{

		background = GetComponent<Image>();
		taskText = GetComponentInChildren<Text>();
		LoadLockTaskState();
		UpdateLockState();

	}
	public void UpdateLockState()
	{
		if (background == null) background = GetComponent<Image>();
		if (!completed)
		{
			background.color = lockedColor;
			taskText.color = lockedColor;
		}
		else
		{
			background.color = unlockedColor;
			taskText.color = unlockedColor;
		}
	}

	private void LoadLockTaskState()
	{
		try
		{
			if (!PlayerPrefs.HasKey("LockTask" + taskID))return;
			completed = bool.Parse(PlayerPrefs.GetString("LockTask" + taskID));
		}
		catch (System.Exception e)
		{
			Debug.Log(e);
		}
	}

	public void SetText(string text)
	{
		if (taskText == null)
		{
			taskText = GetComponentInChildren<Text>();
		}
		taskText.text = text;
	}

	public void SetCompleted(bool b)
	{
		this.completed = b;
	}

	public bool IsCompleted()
	{
		return completed;
	}
}
