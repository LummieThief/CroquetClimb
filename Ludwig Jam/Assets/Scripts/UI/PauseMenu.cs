using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] GameObject pauseMenuUI;
	[SerializeField] GameObject optionsMenuUI;
	[SerializeField] GameObject creditsMenuUI;

	public static bool paused = false;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (creditsMenuUI.activeSelf)
			{
				creditsMenuUI.SetActive(false);
			}
			else if (optionsMenuUI.activeSelf)
			{
				GetComponent<OptionsMenu>().Back();
			}
			else if (!paused)
			{
				Open();
			}
			else
			{
				Resume();
			}
		}
	}

	public void Resume()
	{
		EventHandler.instance.Resume();
		pauseMenuUI.SetActive(false);
		paused = false;
		Time.timeScale = 1;
	}

	public void Open()
	{
		EventHandler.instance.Pause();
		pauseMenuUI.SetActive(true);
		if (!StartMenu.started)
		{
			GetComponent<StartMenu>().OverrideStart();
		}
		paused = true;
		Time.timeScale = 0;
	}

	public void Credits()
	{
		creditsMenuUI.SetActive(true);
	}

	public void Options()
	{
		optionsMenuUI.SetActive(true);
		pauseMenuUI.SetActive(false);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
