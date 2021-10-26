using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotsHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color greatColor, goodColor, badColor;

    // Update is called once per frame
    void Update()
    {
        int shots = BallPhysics.instance.shots;
        text.text = "" + shots;
        if (shots == 0)
        {
            text.color = Color.black;
        }
        else if (shots < 3)
		{
            text.color = badColor;
		}
        else if (shots > 6)
		{
            text.color = greatColor;
		}
        else
		{
            text.color = goodColor;
        }
    }
}
