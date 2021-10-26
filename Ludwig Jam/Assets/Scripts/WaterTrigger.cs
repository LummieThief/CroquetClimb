using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
	[SerializeField] FollowObject cameraRig;

	public static bool sunk;

	private void Awake()
	{
		EventHandler.instance.e_Restart += ListenRestart;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Balls"))
		{
			cameraRig.enabled = false;
			AudioManager.instance.Play("splash");
			other.GetComponent<BallPhysics>().SetShots(0);
			sunk = true;
		}
	}

	public void ListenRestart()
	{
		sunk = false;
		cameraRig.enabled = true;
	}
}
