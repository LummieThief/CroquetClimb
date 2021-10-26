using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallParticles : MonoBehaviour
{
	[SerializeField] ParticleSystem grass, dust, brownGrass;

	private void Awake()
	{
		grass.Play();
		grass.Clear();
		dust.Clear();
		dust.Play();
		brownGrass.Play();
		brownGrass.Clear();
	}

	public void SetParticles(float speed, string ground)
	{
		if (speed > 5 && ground == "Grass")
		{
			var emm = grass.emission;
			emm.rateOverTime = Mathf.Pow((speed / 5), 2);
		}
		else
		{
			var emm = grass.emission;
			emm.rateOverTime = 0;
		}

		if (speed > 5 && ground == "BrownGrass")
		{
			var emm = brownGrass.emission;
			emm.rateOverTime = Mathf.Pow((speed / 5), 2);
		}
		else
		{
			var emm = brownGrass.emission;
			emm.rateOverTime = 0;
		}

		if (speed > 5 && ground == "Stone")
		{
			var emm = dust.emission;
			emm.rateOverTime = Mathf.Pow((speed / 4), 2);
		}
		else
		{
			var emm = dust.emission;
			emm.rateOverTime = 0;
		}

		if (speed > 20)
		{
			var emm = dust.emission;
			emm.rateOverTime = Mathf.Pow((speed / 3), 2);
		}
		else if (ground != "Stone")
		{
			var emm = dust.emission;
			emm.rateOverTime = 0;
		}
	}
}
