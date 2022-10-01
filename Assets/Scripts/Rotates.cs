using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotates : MonoBehaviour
{
    public float degreesPerSecond = 180.0f;

    void Update()
    {
        transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime);
    }
}
