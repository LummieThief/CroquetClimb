using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] Transform target;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
		{
            transform.RotateAround(target.position, Vector3.up, 20f * Time.deltaTime);
		}
    }
}
