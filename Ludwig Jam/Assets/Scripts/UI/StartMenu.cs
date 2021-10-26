using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
	[SerializeField] Animator animator;
	public static bool started;
	private bool stakeTutorialShown;

	private void Awake()
	{
		EventHandler.instance.e_Play += ListenPlay;
		EventHandler.instance.e_Stake += ListenStake;
	}

	public void OverrideStart()
	{
		if (!started)
		{
			animator.SetTrigger("Finish");
			started = true;
		}
	}

	public void ListenPlay()
	{
		if (!started)
		{
			animator.SetTrigger("Slide");
			started = true;
		}
	}

	public void ListenStake()
	{
		if (!stakeTutorialShown)
		{
			animator.SetTrigger("FirstStake");
			stakeTutorialShown = true;
		}
	}
}
