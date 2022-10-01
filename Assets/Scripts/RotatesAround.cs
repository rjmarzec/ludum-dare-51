using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatesAround : MonoBehaviour
{
    public Transform target;
    public float degreesPerSecond;

    private bool doRotateAround = false;

    void Update()
    {
        if(doRotateAround)
        {
			transform.RotateAround(target.position, Vector3.forward, degreesPerSecond * Time.deltaTime);
		}
    }

    public void Reset()
    {
        target = transform;
        degreesPerSecond = 0;

		doRotateAround = false;
	}

    public void SetNewTarget(Transform targetIn, float degreesPerSecondIn)
    {
        target = targetIn;
        degreesPerSecond = degreesPerSecondIn;

		doRotateAround = true;
	}

	public void SetNewTarget(Rotates rotates)
	{
        target = rotates.transform;
        degreesPerSecond = rotates.degreesPerSecond;

		doRotateAround = true;
	}

	public void SetNewTarget(RotatesAround rotatesAround)
	{
		target = rotatesAround.transform;
		degreesPerSecond = rotatesAround.degreesPerSecond;

		doRotateAround = true;
	}
}
