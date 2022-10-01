using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksTargetObject : MonoBehaviour
{
	[Range(0.0f, 1.0f)]
	public float trackingRatePerFrame;
    public GameObject targetGameObject;

    void Update()
    {
        Vector2 offset = (targetGameObject.transform.position - transform.position) * trackingRatePerFrame;
        Vector3 offsetAsVector3 = offset;
		transform.position += offsetAsVector3;
    }
}
