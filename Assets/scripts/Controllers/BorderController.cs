using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour {

	Camera camera;
	Vector3 position;
	float borderThickness = 30f;
	BoxCollider2D collider;
	GameplayController gameplayController;
	PlayerController player;
	float timer;

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
	}


	void Update ()
	{
		if (GameplayController.GAME_STATE != State.GAME) return;

		collider.enabled = !player.activeBoost;
		SetPosition(type);
		transform.gameObject.SetActive(!gameplayController.DEBUG);
	}

	void SetPosition(BorderType type)
	{
		switch (type)
		{
			case BorderType.LEFT:
			{
				position = Camera.main.ViewportToWorldPoint(new Vector3(0, .5f, 0));
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
				position = Camera.main.ViewportToWorldPoint(new Vector3(1, .5f, 0));
				position.z = 0;
				transform.localScale = new Vector3(borderThickness, camera.orthographicSize * 2.0f, 0);
				transform.position = position +  new Vector3(transform.localScale.x * .495f, 0, 0);;
				break;
			}
		}
	}
}
