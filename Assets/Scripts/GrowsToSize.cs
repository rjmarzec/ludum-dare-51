using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowsToSize : MonoBehaviour
{
    public float timeToGrow = 0.25f;
    private Vector3 startingSize;
    private float timer;

    void Start()
    {
        startingSize = transform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime;

        transform.localScale = startingSize * timer/timeToGrow;

        if(timer >= timeToGrow)
        {
            transform.localScale = startingSize;
            Destroy(GetComponent<GrowsToSize>());
        }
    }
}
