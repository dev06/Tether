using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TetherHead : MonoBehaviour {

	private ShopButtonHandler shopButtonHandler;
	private int unlockAt = -1;
	private int id = -1;
	private Sprite spriteImage;
	private Image skin;
	private Image image;
	private Image padlock;
	private Text text;
	private bool locked = true;
	private bool isInit;

	public void Init()
	{
		image = GetComponent<Image>();
		text = transform.GetChild(0).GetComponent<Text>();
		skin = transform.GetChild(1).GetComponent<Image>();
		skin.preserveAspect = true;
		padlock = transform.GetChild(2).GetComponent<Image>();
		SetSprite();
		shopButtonHandler = ShopButtonHandler.Instance;
		if (shopButtonHandler.GetCurrentTetherHead().ID == ID)
		{
			ShowBackground();
		}

		padlock.enabled = locked;
		unlockAt = shopButtonHandler.TetherUnlocks[ID];
		text.text = unlockAt > 0 ? unlockAt.ToString() : "";
		isInit = true;
	}

	public void CheckForIsLocked()
	{
		float bestScore = GameplayController.Instance.BestScore;

		Locked = bestScore < UnlockAt;

		PlayerPrefs.SetString("TetherHead_" + ID, Locked.ToString());

		UpdateLock();
	}

	private void UpdateLock()
	{
		if (!isInit)
		{
			Init();
		}

		if (ID == 0)
		{
			Locked = false;
		}

		padlock.enabled = locked;

		text.text = unlockAt > 0 ? unlockAt.ToString() : "";
	}

	public int ID {
		get {
			return id;
		}
		set {
			this.id = value;
		}
	}

	void SetSprite()
	{
		spriteImage = ShopButtonHandler.Instance.TetherSprites[ID];
		skin.sprite = spriteImage;
	}

	public Sprite sprite
	{
		get {
			return spriteImage;
		}

		set
		{
			this.spriteImage = value;
		}
	}

	public void ShowBackground()
	{
		Background.color = Color.white;
	}

	public int UnlockAt {
		get {
			return unlockAt;
		}

		set {
			this.unlockAt = value;
		}
	}

	public bool Locked
	{
		get {
			return locked;
		}

		set {
			this.locked = value;
		}
	}
	public void HideBackground()
	{
		Background.color = new Color(1, 1, 1, 0f);
	}
	public Image Background
	{
		get {
			if (image == null)
			{
				image = GetComponent<Image>();
			}

			return image;
		}
	}

}
