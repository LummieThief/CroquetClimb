using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] GameObject optionsMenuUI;
    [SerializeField] GameObject pauseMenuUI;

    [SerializeField] AudioMixer musicMixer, effectMixer;

	[SerializeField] Slider musicSlider, effectSlider;
	[SerializeField] Toggle fullscreenToggle, brutalToggle;

	private void Awake()
	{
		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
		}
		if (PlayerPrefs.HasKey("EffectVolume"))
		{
			effectSlider.value = PlayerPrefs.GetFloat("EffectVolume");
		}
		if (PlayerPrefs.HasKey("Fullscreen"))
		{
			fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
		}
		if (PlayerPrefs.HasKey("Brutal"))
		{
			brutalToggle.isOn = PlayerPrefs.GetInt("Brutal") == 1;
		}
	}

	public void Back()
	{
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
	}

    public void ChangeMusicVolume(float newVol)
	{
        musicMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("MusicVolume", newVol);
	}

    public void ChangeEffectVolume(float newVol)
	{
        effectMixer.SetFloat("Volume", Mathf.Log10(newVol) * 20);
		PlayerPrefs.SetFloat("EffectVolume", newVol);
	}

    public void ToggleFullscreen(bool val)
	{
        Screen.fullScreen = val;
		if (val)
		{
			PlayerPrefs.SetInt("Fullscreen", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Fullscreen", 0);
		}
		
	}

	public void ToggleBrutal(bool val)
	{
		StakeTrigger.brutal = val;
		if (val)
		{
			PlayerPrefs.SetInt("Brutal", 1);
		}
		else
		{
			PlayerPrefs.SetInt("Brutal", 0);
		}

	}
}
