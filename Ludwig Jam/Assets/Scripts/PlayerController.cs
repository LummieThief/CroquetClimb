using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallPhysics))]
public class PlayerController : MonoBehaviour
{
	public static PlayerController instance;

    private BallPhysics ball;

	private Vector2 mouseDisplacement;
	[SerializeField] float dragSensitivity;
	[SerializeField] float maxHitForce;
	[SerializeField] float minHitForce;
	
	[SerializeField] IndicatorArrow arrow;
	[SerializeField] MalletController mallet;

	private bool dragging;

	private int shots = 1;
	private Vector3 originalPosition;


	private void Awake()
	{
		SetupSingleton();
		originalPosition = transform.position;
		ball = GetComponent<BallPhysics>();
	}

	void SetupSingleton()
	{
		if (instance != null)
			Destroy(this);
		instance = this;
	}

	void Update()
	{
		DragHitLoop();
	}

	private void DragHitLoop()
	{
		if (ball.shots <= 0 || Winners.won || PauseMenu.paused)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			OnDragStart();
		}

		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			OnDragStop();
		}

		if (dragging)
		{
			
			Vector2 mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			mouseDisplacement -= mouseMovement;

			if (!StartMenu.started)
			{
				if (mouseDisplacement.magnitude > 5)
				{
					EventHandler.instance.Play();
				}
				else
				{
					if (Input.GetKeyUp(KeyCode.Mouse0))
					{
						OnDragStop();
					}
					return;
				}
				
			}

			if (mouseDisplacement.magnitude > dragSensitivity)
			{
				mouseDisplacement = mouseDisplacement.normalized * dragSensitivity;
			}
			
			Vector3 mouseDisplacement3d = new Vector3(mouseDisplacement.x, 0, mouseDisplacement.y);
			if (shots > 0)
			{
				arrow.Point(ball.transform.position, ball.transform.position + mouseDisplacement3d / dragSensitivity * arrow.GetMaxLength());
				mallet.SetSwingState(mouseDisplacement3d.magnitude / dragSensitivity, mouseDisplacement3d);
			}
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				if (shots > 0)
				{
					OnHit();
				}
				OnDragStop();
			}
		}
	}

	// called when the player starts dragging
	private void OnDragStart()
	{
		mouseDisplacement = Vector2.zero;
		dragging = true;
		RefreshArrow();
	}

	// called when the player stops dragging
	private void OnDragStop()
	{
		dragging = false;
		arrow.SetVisible(false);
		if (!mallet.swung)
		{
			mallet.SetVisible(false);
		}
	}

	// called when the player hits the ball
	private void OnHit()
	{
		if (Vector3.Magnitude(arrow.head - arrow.tail) / arrow.GetMaxLength() * maxHitForce < minHitForce)
		{
			return;
		}

		ball.HitBall((arrow.head - arrow.tail) / arrow.GetMaxLength() * maxHitForce);
		mallet.Swing();
		//ball.HitBall((arrow.head - arrow.tail).magnitude * Vector3.back / maxArrowLength * maxHitForce);
		//Debug.Log(Vector3.Magnitude(arrow.head - arrow.tail) / maxArrowLength * maxHitForce);
		//shots--;
		//EventHandler.instance.ShotsUpdated(shots);
	}

	private void RefreshArrow()
	{
		//return;
		if (shots > 0 && dragging)
		{
			arrow.SetVisible(true);
		}
		
	}
}
