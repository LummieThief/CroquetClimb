using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

	private void Awake()
	{
        SetupSingleton();
	}

    void SetupSingleton()
	{
        if (instance != null)
            Destroy(this);
        instance = this;
    }

	public event Action e_Restart;
	public void Restart()
	{
		if (e_Restart != null)
			e_Restart();
	}

	public event Action e_Win;
	public void Win()
	{
		if (e_Win != null)
			e_Win();
	}

	public event Action e_Pause;
	public void Pause()
	{
		if (e_Pause != null)
			e_Pause();
	}

	public event Action e_Resume;
	public void Resume()
	{
		if (e_Resume != null)
			e_Resume();
	}

	public event Action e_Play;
	public void Play()
	{
		if (e_Play != null)
			e_Play();
	}

	public event Action e_Stake;
	public void Stake()
	{
		if (e_Stake != null)
			e_Stake();
	}

	public event Action e_RNG;
	public void RNG()
	{
		if (e_RNG != null)
			e_RNG();
	}
}
