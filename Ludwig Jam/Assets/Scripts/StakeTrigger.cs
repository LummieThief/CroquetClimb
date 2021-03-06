using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeTrigger : MonoBehaviour
{
    private bool dead;
	private int shotsAtStake;
	public static bool brutal;
	[SerializeField] int shots = 6;
	[SerializeField] int brutalShots = 1;

	[SerializeField] Transform number, brutalNumber;
	[SerializeField] GameObject flag;
	[SerializeField] bool winningStake;
	[SerializeField] bool tutorialStake;
	private Vector3 numberOrigin;
	private float numberRiseSpeed = 0.2f;

	private void Awake()
	{
		flag.SetActive(true);
		EventHandler.instance.e_Restart += ListenRestart;
		EventHandler.instance.e_RNG += ListenRestart;

		number.rotation = Quaternion.identity;
		number.Rotate(Vector3.up, 180);

		brutalNumber.rotation = Quaternion.identity;
		brutalNumber.Rotate(Vector3.up, 180);
		numberOrigin = number.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Balls"))
		{
			if (winningStake)
			{
				EventHandler.instance.Win();
				flag.SetActive(false);
			}
			else
			{
				BallPhysics ball = other.GetComponent<BallPhysics>();
				if (!dead)
				{
					if (brutal)
					{
						ball.AddShots(brutalShots);
						StartCoroutine("c_ShowBrutalNumber", 1);
						AudioManager.instance.Play("wicketCollect");
					}
					else
					{
						ball.AddShots(shots);
						StartCoroutine("c_ShowNumber", 1);
						AudioManager.instance.Play("stakeCollect");
					}
					shotsAtStake = ball.shots;
					dead = true;
					flag.SetActive(false);
					/*
					if (tutorialStake)
						EventHandler.instance.Stake();
					*/
				}	
			}
		}
	}

	public void ListenRestart()
	{
		dead = false;
		flag.SetActive(true);
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

	private IEnumerator c_ShowBrutalNumber(float duration)
	{
		brutalNumber.gameObject.SetActive(true);
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			brutalNumber.Translate(Vector3.up * Time.deltaTime * numberRiseSpeed);
			yield return null;
		}
		brutalNumber.position = numberOrigin;
		brutalNumber.gameObject.SetActive(false);

		yield break;
	}
}
