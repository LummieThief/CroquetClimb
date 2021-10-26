using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RestartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    [SerializeField] Button button;
    [SerializeField] GameObject restartText;

    private bool spinning;
    private bool outOfShots;

    private float timer;
    private float timeBeforeRestart = 3f;
    private float timeToDetermineBallIsStopped = 0.5f;

	private void Awake()
	{
        EventHandler.instance.e_Restart += ListenRestart;
	}

	// When highlighted with mouse.
	public void OnPointerEnter(PointerEventData eventData)
    {
        spinning = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        spinning = false;
    }

	private void Update()
	{
        if (BallPhysics.instance.shots <= 0 && (!BallPhysics.instance.rolling || WaterTrigger.sunk))
		{
            timer += Time.deltaTime;
            spinning = true;
            if (timer > timeBeforeRestart)
            {
                restartText.SetActive(true);
            }
            else if (timer > timeToDetermineBallIsStopped)
			{
                outOfShots = true;
            }
        }
        else if (outOfShots)
		{
            spinning = false;
            outOfShots = false;
            restartText.SetActive(false);
            timer = 0;
		}

        if (spinning)
        {
            transform.RotateAround(transform.position, Vector3.forward, 360f * Time.deltaTime);
        }
	}

    public void ListenRestart()
	{
        spinning = false;
        timer = 0;
        outOfShots = false;
        restartText.SetActive(false);
        transform.rotation = Quaternion.identity;
    }

    public void Restart()
	{
        EventHandler.instance.Restart();
	}
}
