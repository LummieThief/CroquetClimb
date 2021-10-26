using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 origin;

    void Awake()
    {
        origin = transform.localPosition;
    }

    public void Shake(float time, float magnitude)
	{
		object[] parames = new object[2] { time, magnitude };
		StartCoroutine("c_Shake", parames);
	}

	private IEnumerator c_Shake(object[] parames)
	{
		float duration = (float)parames[0];
		float mag = (float)parames[1];
		float currentTime = 0;

		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			transform.localPosition = origin + new Vector3(Random.Range(-mag, mag), Random.Range(-mag, mag), Random.Range(-mag, mag));
			yield return null;
		}
		transform.localPosition = origin;
		yield break;
	}
}
