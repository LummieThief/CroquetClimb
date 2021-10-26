using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FollowObject : MonoBehaviour
{
    public Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] bool resetOffset;
    [SerializeField] bool following;
    [SerializeField] bool smooth;
    [SerializeField] float lerpSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void LateUpdate()
    {
        if (resetOffset)
		{
            resetOffset = false;
            offset = transform.position - target.position;
        }
        if (following)
        {
            if (smooth)
            {
                transform.position = Vector3.Lerp(transform.position, target.position + offset, lerpSpeed * Time.deltaTime);
            }
            else
            {
                Physics.SyncTransforms();
                transform.position = target.position + offset;
            }
        }
    }
}
