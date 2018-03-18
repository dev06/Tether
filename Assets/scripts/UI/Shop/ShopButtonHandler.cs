using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopButtonHandler : MonoBehaviour {

	public List<Sprite> TetherSprites = new List<Sprite>();
	public List<TetherHead> TetherHeads = new List<TetherHead>();
	public List<int> TetherUnlocks = new List<int>();
	public static ShopButtonHandler Instance;
	public static TetherHead Current;
	public Transform tetherHeadParent;
	private Image currentHead;
	private ShopPanel shopPanel;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			DestroyImmediate(gameObject);
		}
		AddTetherHeads();

		PlayerPrefs.SetString("TetherHead_0", "False");
		// PlayerPrefs.SetString("TetherHead_1", "False");
		//PlayerPrefs.DeleteAll();
	}

	void AddTetherHeads()
	{
		for (int i = 0; i < tetherHeadParent.childCount; i++)
		{
			TetherHead tt = tetherHeadParent.GetChild(i).GetComponent<TetherHead>();
			tt.transform.name = "Head_" + i;
			tt.ID = i;
			TetherHeads.Add(tt);
		}

		foreach (TetherHead t in TetherHeads)
		{
			t.Init();
			t.CheckForIsLocked();
		}
	}

	void Start ()
	{
		currentHead = transform.GetChild(0).GetComponent<Image>();
		shopPanel = FindObjectOfType<ShopPanel>();
		currentHead.sprite = GetCurrentTetherHead().sprite;
	}
	void OnEnable()
	{
		EventManager.OnTetherHeadClick += OnTetherHeadClick;
	}
	void OnDisable()
	{
		EventManager.OnTetherHeadClick -= OnTetherHeadClick;
	}

	void OnTetherHeadClick(TetherHead t)
	{

		if (t.Locked) return;
		SetCurrentHead(t);

		GameObject[] tetherHeads = GameObject.FindGameObjectsWithTag("UI/Head");
		foreach (GameObject g in tetherHeads)
		{
			TetherHead tt = g.GetComponent<TetherHead>();
			if (!tt.Locked)
			{
				if (tt.ID != Current.ID)
				{
					tt.HideBackground();
				}
				else
				{
					tt.ShowBackground();
				}
			}
		}
	}

	public TetherHead GetCurrentTetherHead()
	{
		int id = !PlayerPrefs.HasKey("CurrentTetherHeadID") ? 0 : PlayerPrefs.GetInt("CurrentTetherHeadID");
		return (Current != null) ? Current : TetherHeads[id];
	}

	public void SetCurrentHead(TetherHead t)
	{
		if (t == null)
		{
			Debug.Log("t is null");
			return;
		}

		Current = t;
		currentHead.sprite = t.sprite;
	}

	public ShopPanel ShopPanel
	{
		get {
			return shopPanel;
		}
	}
}
