using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WicketTrigger : MonoBehaviour
{
	private Transform ball;
	private bool lastSide = true;
	[SerializeField] Transform number;
	private Vector3 numberOrigin;
	private float numberRiseSpeed = 0.2f;

	private void Awake()
	{
		number.rotation = Quaternion.identity;
		number.Rotate(Vector3.up, 180);
		numberOrigin = number.position;

		EventHandler.instance.e_Restart += ListenRestart;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (ball == null && other.gameObject.layer == LayerMask.NameToLayer("Balls"))
		{
			lastSide = GetSide(other.transform.position);
			ball = other.transform;
		}
	}

	private void Update()
	{
		if (ball != null)
		{
			if (GetSide(ball.position) != lastSide && !lastSide)
			{
				
				ball.GetComponent<BallPhysics>().AddShots(1);
				StartCoroutine("c_ShowNumber", 1);
				if (AudioManager.instance.GetSource("wicketCollect").isPlaying)
				{
					AudioManager.instance.Play("wicketCollect2");
				}
				else
				{
					AudioManager.instance.Play("wicketCollect");
				}
				
			}
			lastSide = GetSide(ball.position);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (ball != null && other.gameObject.layer == LayerMask.NameToLayer("Balls"))
		{
			ball = null;
		}
	}


	private bool GetSide(Vector3 playerPosition)
	{
		Vector3 toPlayer = (playerPosition - transform.position).normalized;
		bool side = Vector3.Dot(toPlayer, transform.forward) > 0;
		return side;
	}

	private IEnumerator c_ShowNumber(float duration)
	{
		number.gameObject.SetActive(true);
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			number.Translate(Vector3.up * Time.deltaTime * numberRiseSpeed);
			yield return null;
		}
		number.position = numberOrigin;
		number.gameObject.SetActive(false);
		yield break;
	}

	public void ListenRestart()
	{
		ball = null;
		lastSide = true;
	}
}