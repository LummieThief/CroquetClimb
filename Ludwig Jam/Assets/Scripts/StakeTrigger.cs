using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeTrigger : MonoBehaviour
{
    private bool dead;
	public static bool brutal;
	[SerializeField] int shots = 6;
	[SerializeField] int brutalShots = 1;

	[SerializeField] Transform number, brutalNumber;
	[SerializeField] GameObject flag;
	[SerializeField] bool winningStake;
	private Vector3 numberOrigin;
	private float numberRiseSpeed = 0.2f;

	private void Awake()
	{
		EventHandler.instance.e_Restart += ListenRestart;

		number.rotation = Quaternion.identity;
		number.Rotate(Vector3.up, 180);

		brutalNumber.rotation = Quaternion.identity;
		brutalNumber.Rotate(Vector3.up, 180);
		numberOrigin = number.position;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!dead && other.gameObject.layer == LayerMask.NameToLayer("Balls"))
		{
			if (winningStake)
			{
				EventHandler.instance.Win();
				flag.SetActive(true);
			}
			else
			{
				if (brutal)
				{
					other.GetComponent<BallPhysics>().AddShots(brutalShots);
					StartCoroutine("c_ShowBrutalNumber", 1);
					AudioManager.instance.Play("wicketCollect");
				}
				else
				{
					other.GetComponent<BallPhysics>().AddShots(shots);
					StartCoroutine("c_ShowNumber", 1);
					AudioManager.instance.Play("stakeCollect");
				}
				
				dead = true;
				
				flag.SetActive(true);
			}
		}
	}

	public void ListenRestart()
	{
		dead = false;
		flag.SetActive(false);
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
