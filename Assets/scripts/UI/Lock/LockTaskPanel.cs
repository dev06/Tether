using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class LockTaskPanel : MonoBehaviour {

	private static bool DEBUG; 

	GameplayController gameplayController; 

	private bool AllTaskComplete = false; 

	private bool active; 

	private CanvasGroup group; 

	List<LockTask> lockTasks = new List<LockTask>(); 

	void Start () 
	{
		gameplayController = GameplayController.Instance; 

		group = GetComponent<CanvasGroup>(); 

		GameObject[] tasks = GameObject.FindGameObjectsWithTag("Task"); 

		foreach(GameObject t in tasks)
		{
			lockTasks.Add(t.GetComponent<LockTask>()); 
		}

		if(PlayerPrefs.HasKey("AllTaskCompleted"))
		{
			AllTaskComplete = bool.Parse(PlayerPrefs.GetString("AllTaskCompleted")); 
		}

		CanDeactivePanel(); 

		SetTaskText(); 

	}

	void OnApplicationQuit()
	{

	}

	private void SetTaskText()
	{
		if(AllTaskComplete) return; 
		foreach(LockTask t in lockTasks)
		{
			switch(t.taskID)
			{
				case LockTaskID.ID_1:
				{
					t.SetText("Get a score of " + LockTaskValue.Task1Value); 
					break; 
				}

				case LockTaskID.ID_2:
				{
					t.SetText("Tether to " + LockTaskValue.Task2Value + " bases consecutively"); 
					break; 
				}

				case LockTaskID.ID_3:
				{
					t.SetText("Get a score of " + LockTaskValue.Task3Value + " without using a boost"); 
					break; 
				}
			}
		}
	}


	private void CanDeactivePanel()
	{
		if(AllTaskComplete || DEBUG)
		{
			group.alpha = 0; 
			group.blocksRaycasts = false; 

		}
		active = group.alpha == 1; 
	}

	public void InvokeLockTask(LockTaskID id)
	{	
		if(GetLockTask(id).IsCompleted()) return; 

		if(AllTaskComplete) return;

		if(EventManager.OnLockTaskComplete != null)
		{
			EventManager.OnLockTaskComplete(id); 
		}


		AllTaskComplete = AreAllTaskCompleted(); 

		PlayerPrefs.SetString("AllTaskCompleted", AllTaskComplete.ToString()); 
	}

	private bool AreAllTaskCompleted()
	{
		foreach(LockTask t in lockTasks)
		{
			if(!t.IsCompleted()) return false; 
		}
		return true; 
	}

	private LockTask GetLockTask(LockTaskID id)
	{
		foreach(LockTask l in lockTasks)
		{
			if(l.taskID == id) return l; 
		}

		return null; 
	}

	public bool Active{
		get{
			return active; 
		}
	}

	public bool GetAllTaskComplete
	{
		get{
			return AllTaskComplete; 
		}
	}
}
