using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimsLightOverTime : MonoBehaviour
{
    private Light dimmingLight;
    private float timer;
    private float startingIntensity;
    private float startingRange;

	public float timeToDim = 10.0f;

	void Start()
    {
        dimmingLight = GetComponent<Light>();
        timer = 0;
        startingIntensity = dimmingLight.intensity;
        startingRange = dimmingLight.range;
    }

    void Update()
    {
        timer += Time.deltaTime;

        float dimmingFactor = (timeToDim - timer) / timeToDim;
		dimmingLight.intensity = startingIntensity * dimmingFactor * dimmingFactor;
        dimmingLight.range = startingRange * dimmingFactor * dimmingFactor;

        if(timer >= timeToDim)
        {
            Destroy(gameObject);
		}
    }
}
