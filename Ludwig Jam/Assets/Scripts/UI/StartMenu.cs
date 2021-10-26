using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
	[SerializeField] Animator animator;
	public static bool started;

	private void Awake()
	{
		EventHandler.instance.e_Play += ListenPlay;
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
}
