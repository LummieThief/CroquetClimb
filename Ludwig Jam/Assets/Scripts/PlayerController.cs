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
	[SerializeField] float rotationSpeed;
	
	[SerializeField] IndicatorArrow arrow;
	[SerializeField] MalletController mallet;

	[SerializeField] Transform number;
	private Vector3 numberOffset = Vector3.up;
	private float numberRiseSpeed = 0.2f;

	private AudioSource spin;

	private bool rngHit;


	private bool dragging;

	private int shots = 1;
	private Vector3 originalPosition;


	private void Awake()
	{
		SetupSingleton();
		originalPosition = transform.position;
		ball = GetComponent<BallPhysics>();

		EventHandler.instance.e_Restart += ListenRestart;
	}

	void SetupSingleton()
	{
		if (instance != null)
			Destroy(this);
		instance = this;
	}

	void Update()
	{
		if (Winners.won || PauseMenu.paused)
		{
			return;
		}
		else if (ball.shots > 0)
		{
			DragHitLoop();
		}
		else
		{
			OutOfShotsLoop();
		}
	}

	private void DragHitLoop()
	{
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

			arrow.Point(ball.transform.position, ball.transform.position + mouseDisplacement3d / dragSensitivity * arrow.GetMaxLength());
			mallet.SetSwingState(mouseDisplacement3d.magnitude / dragSensitivity, mouseDisplacement3d);

			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				OnHit();
				OnDragStop();
			}
		}

		if (rngHit && !ball.rolling)
		{
			rngHit = false;
			StartCoroutine("c_ShowNumber", 1);
			AudioManager.instance.Play("wicketCollect");
			ball.AddShots(2);
			return;
		}
	}

	private void OutOfShotsLoop()
	{
		if (spin == null)
		{
			spin = AudioManager.instance.GetSource("spin");
		}
		if (rngHit && !ball.rolling)
		{
			rngHit = false;
			StartCoroutine("c_ShowNumber", 1);
			AudioManager.instance.Play("wicketCollect");
			ball.AddShots(2);
			return;
		}
		else if (ball.rolling) 
		{
			if (spin.isPlaying)
			{
				spin.Stop();
			}
			return; 
		}

		if (!spin.isPlaying) 
		{
			spin.Play();
			EventHandler.instance.RNG();
		}

		OnDragStart();
		Vector3 endPoint = ball.transform.position + new Vector3(1, 0, 0) * arrow.GetMaxLength();
		endPoint = RotatePointAroundPivot(endPoint, ball.transform.position, Vector3.up * ((Time.time * rotationSpeed) % 360));
		arrow.Point(ball.transform.position, endPoint);
		mallet.SetSwingState(1, endPoint - ball.transform.position);

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			OnHit();
			rngHit = true;
			ball.AddShots(1);
			OnDragStop();
		}
	}

	// called when the player starts dragging
	private void OnDragStart()
	{
		if (dragging)
			return;
		mouseDisplacement = Vector2.zero;
		dragging = true;
		RefreshArrow();
	}

	// called when the player stops dragging
	private void OnDragStop()
	{
		if (!dragging)
			return;
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

	private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}

	public void ListenRestart()
	{
		rngHit = false;
	}

	private IEnumerator c_ShowNumber(float duration)
	{
		number.position = ball.transform.position + numberOffset;
		number.gameObject.SetActive(true);
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			number.Translate(Vector3.up * Time.deltaTime * numberRiseSpeed);
			yield return null;
		}
		
		number.gameObject.SetActive(false);

		yield break;
	}
}
