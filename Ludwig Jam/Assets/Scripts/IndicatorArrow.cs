using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorArrow : MonoBehaviour
{
    [SerializeField] LineRenderer line;
	[SerializeField] Color arrowStartColor, arrowEndColor;
	[SerializeField] float maxLength;
	public Vector3 tail { get; private set; }
	public Vector3 head { get; private set; }

	public void Point(Vector3 from, Vector3 to)
	{
		head = to;
		tail = from;
        line.SetPosition(0, tail);
        line.SetPosition(1, head);

		Color c = Color.Lerp(arrowStartColor, arrowEndColor, Vector3.Magnitude(head - tail) / maxLength);
		line.startColor = c;
		line.endColor = c;
	}

	public void SetVisible(bool val)
	{
		line.enabled = val;
	}

	public float GetMaxLength()
	{
		return maxLength;
	}
}
