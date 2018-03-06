using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour {


	private Camera camera;

	private Vector3 position;

	private float borderThickness = 30f;

	private BoxCollider2D collider;

	private GameplayController gameplayController;

	private PlayerController player;

	private float timer;

	private float startOffset;

	private float targetOffset;

	private float offsetSpeed;

	public enum BorderType
	{
		LEFT,
		RIGHT,
		TOP,
		BOTTOM,
	}

	public BorderType type;

	void Start ()
	{

		camera = Camera.main;

		gameplayController = GameplayController.Instance;

		collider = GetComponent<BoxCollider2D>();

		collider.enabled = false;

		player = PlayerController.Instance;

		startOffset = type == BorderType.LEFT ? -.2f : 1.2f;

		targetOffset = type == BorderType.LEFT ? 0f : 1f;
	}


	void Update ()
	{
		if (GameplayController.GAME_STATE != State.GAME || gameplayController.inTutorial) { return; }

		if (Mathf.Abs(targetOffset - startOffset) > .01f)
		{
			startOffset = Mathf.SmoothDamp(startOffset, targetOffset , ref offsetSpeed, Time.unscaledDeltaTime * 15f);
		}
		else
		{
			startOffset = targetOffset;
		}


		collider.enabled = !player.activeBoost && !gameplayController.inTutorial;

		SetPosition(type);

		transform.gameObject.SetActive(!gameplayController.DEBUG);
	}

	void SetPosition(BorderType type)
	{
		switch (type)
		{
			case BorderType.LEFT:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(startOffset, .5f, 0));
				position.z = 0;
				transform.localScale = new Vector3(borderThickness, camera.orthographicSize * 2.0f, 0);
				transform.position = position - new Vector3(transform.localScale.x * .495f, 0, 0);
				break;
			}
			// case BorderType.BOTTOM:
			// {
			// 	position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 0, 0));
			// 	position.z = 0;
			// 	transform.localScale = new Vector3(borderThickness, camera.orthographicSize, 0);
			// 	transform.position = position - new Vector3(0, transform.localScale.x * .32f, 0);
			// 	break;
			// }
			// case BorderType.TOP:
			// {
			// 	position = Camera.main.ViewportToWorldPoint(new Vector3(.5f, 1, 0));
			// 	position.z = 0;
			// 	transform.localScale = new Vector3(borderThickness, camera.orthographicSize, 0);
			// 	transform.position = position + new Vector3(0, transform.localScale.x * .32f, 0);
			// 	break;
			// }
			case BorderType.RIGHT:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(startOffset, .5f, 0));
				position.z = 0;
				transform.localScale = new Vector3(borderThickness, camera.orthographicSize * 2.0f, 0);
				transform.position = position +  new Vector3(transform.localScale.x * .495f, 0, 0);;
				break;
			}
		}
	}
}
