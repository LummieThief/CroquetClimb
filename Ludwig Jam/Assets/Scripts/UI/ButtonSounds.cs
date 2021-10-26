using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
	public void MouseEnter()
	{
		AudioManager.instance.Play("popUp");
	}

	public void MouseExit()
	{
		AudioManager.instance.Play("popDown");
	}

	public void MouseDown()
	{
		AudioManager.instance.Play("popDown");
	}
}
