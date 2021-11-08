using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMusicController : MonoBehaviour
{
	private AudioSource musicLoop;

	private bool paused = false;
	private bool unpauseOnResume = false;

	private void Awake()
	{
		EventHandler.instance.e_Win += ListenWin;
		EventHandler.instance.e_Restart += ListenRestart;
		EventHandler.instance.e_Pause += ListenPause;
		EventHandler.instance.e_Resume += ListenResume;
		musicLoop = AudioManager.instance.GetSource("loop1");
		musicLoop.Play();
	}

	private void Update()
	{
		if (Winners.won)
		{
			return;
		}

		if ((BallPhysics.instance.shots <= 0 && !BallPhysics.instance.rolling) || WaterTrigger.sunk)
		{
			musicLoop.mute = true;
			paused = true;
			return;
		}

		if (paused)
		{
			if (BallPhysics.instance.shots > 0)
			{
				//musicLoop.UnPause();
				musicLoop.mute = false;
				paused = false;
			}
			else
			{
				return;
			}
		}
	}

	public void ListenWin()
	{
		musicLoop.Stop();
		AudioManager.instance.Play("tiny1");
	}
	public void ListenRestart()
	{
		/*
		if (!musicLoop.isPlaying)
		{
			musicLoop.UnPause();
		}
		paused = false;
		*/
		musicLoop.mute = false;
		musicLoop.Stop();
		musicLoop.Play();
		paused = false;
	}

	public void ListenPause()
	{
		/*
		if (musicLoop.isPlaying)
		{
			musicLoop.Pause();
			unpauseOnResume = true;
		}
		*/

		if (!musicLoop.mute)
		{
			musicLoop.mute = true;
			unpauseOnResume = true;
		}
	}
	public void ListenResume()
	{
		/*
		if (unpauseOnResume)
		{
			musicLoop.UnPause();
		}
		unpauseOnResume = false;
		*/

		if (unpauseOnResume)
		{
			musicLoop.mute = false;
		}
		unpauseOnResume = false;
	}
}
