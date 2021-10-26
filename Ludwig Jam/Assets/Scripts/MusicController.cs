using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
	[SerializeField] BallPhysics ball;
	[SerializeField] float minHeight;
	[SerializeField] float maxHeight;
	[SerializeField] float mThreshold, sThreshold, tThreshold;
	private float mPercent, sPercent, tPercent;
	private float currentHeight;
	private float percentage;

	[SerializeField] float maxSilenceDuration;
	private float silence = 0;
	private float silenceDuration = 0;
	private AudioSource currentTrack;

	private bool paused = false;
	private bool unpauseOnResume = false;

	private void Awake()
	{
		mPercent = mThreshold / maxHeight;
		sPercent = sThreshold / maxHeight;
		tPercent = tThreshold / maxHeight;

		EventHandler.instance.e_Win += ListenWin;
		EventHandler.instance.e_Restart += ListenRestart;
		EventHandler.instance.e_Pause += ListenPause;
		EventHandler.instance.e_Resume += ListenResume;
		currentTrack = AudioManager.instance.GetSource("loop1");
		currentTrack.Play();
	}

	private void Update()
	{
		if (Winners.won)
		{
			return;
		}

		if (paused)
		{
			if (BallPhysics.instance.shots > 0)
			{
				currentTrack.UnPause();
				paused = false;
			}
			else
			{
				return;
			}
		}

		

		currentHeight = Mathf.Clamp(ball.transform.position.y, minHeight, maxHeight * 2);
		percentage = currentHeight / maxHeight;
		
		if (currentTrack == null)
		{
			StartNextTrack();
		}
		else if ((BallPhysics.instance.shots <= 0 && !BallPhysics.instance.rolling) || WaterTrigger.sunk)
		{
			currentTrack.Pause();
			paused = true;
			return;
		}
		else if (!currentTrack.isPlaying && !PauseMenu.paused && silence == 0)
		{
			silenceDuration = Random.Range(1, maxSilenceDuration * percentage);
		}

		if (silenceDuration > 0)
		{
			silence += Time.deltaTime;
			if (silence > silenceDuration)
			{
				silenceDuration = 0;
				silence = 0;
				StartNextTrack();
			}
		}
		
	}

	private void StartNextTrack()
	{
		
		string nextTrackName = PickTrack();
		if (nextTrackName.Length <= 1)
		{
			return;
		}
		currentTrack = AudioManager.instance.GetSource(nextTrackName);
		AudioManager.instance.Play(nextTrackName);
	}

	private string PickTrack()
	{
		string track = "";
		float l = Random.Range(0f, 1f);
		float m = Random.Range(mPercent, 1f);
		float s = Random.Range(sPercent, 1f);
		float t = Random.Range(tPercent, 1f);

		

		if (percentage < mPercent)
		{
			track += "long";
		}
		else if (percentage < sPercent)
		{
			if (l > m)
				track += "long";
			else
				track += "med";
		}
		else if (percentage < tPercent)
		{
			if (l > m && l > s)
				track += "long";
			else if (m > l && m > s)
				track += "med";
			else
				track += "short";
		}
		else if (percentage < 1)
		{
			if (l > m && l > s && l > t)
				track += "long";
			else if (m > l && m > s && m > t)
				track += "med";
			else if (s > l && s > m && s > t)
				track += "short";
			else
				track += "tiny";
		}
		else
		{
			silence = 0;
			silenceDuration = maxSilenceDuration;
			return "";
		}

		track += Random.Range(1, 3);

		return track;
	}

	public void ListenWin()
	{
		if (currentTrack != null)
		{
			currentTrack.Stop();
		}
		
		AudioManager.instance.Play("tiny1");
	}
	public void ListenRestart()
	{
		if (currentTrack != null)
		{
			currentTrack.Stop();
			currentTrack = null;
		}
		silence = 0;
		silenceDuration = 0;
		paused = false;
	}

	public void ListenPause()
	{
		if (currentTrack != null)
		{
			if (currentTrack.isPlaying)
			{
				currentTrack.Pause();
				unpauseOnResume = true;
			}
		}
	}
	public void ListenResume()
	{
		if (currentTrack != null && unpauseOnResume)
		{
			currentTrack.UnPause();
		}
		unpauseOnResume = false;
	}
}
