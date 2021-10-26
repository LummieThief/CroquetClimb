using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalletController : MonoBehaviour
{

    [SerializeField] float minAngleX;
    [SerializeField] float maxAngleX;
    [SerializeField] float hitAngleX;
    [SerializeField] List<MeshRenderer> malletRenderers;
    //[SerializeField] ParticleSystem smokePuff;
    private float currentAngleX;
    private Vector3 facingDirection;
    public bool swung { get; private set; }

    private float timer;
    private float maxTimer = 1f;

    private bool visible = true;

    private FollowObject follow;

	private void Awake()
	{
        follow = GetComponent<FollowObject>();
        SetVisible(false);
	}

	private void Update()
	{
		if (swung)
		{
            timer += Time.deltaTime;
            transform.Translate(Time.deltaTime * Vector3.up * Mathf.Pow(timer * 4, 4));
            if (timer > maxTimer)
			{
                timer = 0;
                swung = false;
                SetVisible(false);
			}
		}

	}

	public void SetSwingState(float swingPercentage, Vector3 direction)
	{
        currentAngleX = minAngleX + (maxAngleX - minAngleX) * swingPercentage;
        facingDirection = direction;
        Refresh();
    }
    public void SetSwingPercentage(float p)
	{
        
        currentAngleX = minAngleX + (maxAngleX - minAngleX) * p;
        Refresh();
    }
    public void SetDirection(Vector3 dir)
	{
        facingDirection = dir;
        Refresh();
	}
    
    private void Refresh()
	{
        if (facingDirection == Vector3.zero)
		{
            return;
		}
        transform.rotation = Quaternion.LookRotation(facingDirection, Vector3.up);
        transform.eulerAngles = new Vector3(currentAngleX, transform.eulerAngles.y, transform.eulerAngles.z);
        SetVisible(true);
        follow.enabled = true;
        swung = false;
        timer = 0;
    }
    
    public void SetVisible(bool v)
	{
        if (v == visible)
		{
            return;
		}
        else
		{
            visible = v;
            foreach (MeshRenderer m in malletRenderers)
            {
                m.enabled = v;
            }
        }
        
    }

    public void Swing()
	{
        currentAngleX = hitAngleX;
        Refresh();
        follow.enabled = false;
        swung = true;
        // smokePuff.Play();
	}
}
