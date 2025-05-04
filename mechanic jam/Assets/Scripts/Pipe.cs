using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
	public char type;
	public bool hasWater = false;

	// rotates the pipe 90 degrees clockwise if dir is true, and 90 degrees
	// counter clockwise if dir is false
	public void Rotate(bool dir)
	{
		if (dir)
		{
			transform.Rotate(Vector3.up, 90);
		}
		else
		{
			transform.Rotate(Vector3.up, -90);
		}

	}

	public void Empty()
	{
		if (!hasWater)
			return;

		foreach (ParticleSystem s in GetComponentsInChildren<ParticleSystem>())
		{
			s.Stop();
		}

		hasWater = false;
	}

	public void Fill()
	{
		if (hasWater)
			return;

		foreach (ParticleSystem s in GetComponentsInChildren<ParticleSystem>())
		{
			s.Play();
		}

		hasWater = true;
		for (int i = 0; i < 4; i++)
		{
			if (HasCompleteConnection(i))
			{
				RaycastHit hit;
				int mask = LayerMask.GetMask("BB");
				if (Physics.Raycast(GetRayOut(i), out hit, 100f, mask))
				{
					hit.collider.gameObject.GetComponent<Pipe>().Fill();
				}
			}
		}
	}

	private bool HasCompleteConnection(int rot) {
		Ray outRay = GetRayOut(rot);
		Ray inRay = GetRayIn(rot);

		return RayHitsPipe(outRay) && RayHitsPipe(inRay);
	}

	private bool RayHitsPipe(Ray ray)
	{
		int mask = LayerMask.GetMask("Pipes");
		return Physics.Raycast(ray, 0.3f, mask);
	}

	private Ray GetRayOut(int rot)
	{
		switch (rot)
		{
			case 0:
				return new Ray(transform.position + Vector3.forward * 0.9f, Vector3.forward);
			case 1:
				return new Ray(transform.position + Vector3.right * 0.9f, Vector3.right);
			case 2:
				return new Ray(transform.position + Vector3.back * 0.9f, Vector3.back);
			case 3:
				return new Ray(transform.position + Vector3.left * 0.9f, Vector3.left);
		}
		return new Ray();
	}

	private Ray GetRayIn(int rot)
	{
		switch (rot)
		{
			case 0:
				return new Ray(transform.position + Vector3.forward * 1.1f, Vector3.back);
			case 1:
				return new Ray(transform.position + Vector3.right * 1.1f, Vector3.left);
			case 2:
				return new Ray(transform.position + Vector3.back * 1.1f, Vector3.forward);
			case 3:
				return new Ray(transform.position + Vector3.left * 1.1f, Vector3.right);
		}
		return new Ray();
	}
}
