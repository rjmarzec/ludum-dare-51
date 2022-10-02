using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spins : MonoBehaviour
{
    private Vector3 rotationSpeed;

    void Start()
    {
        // randomly assign the rotation speed along each axis
        rotationSpeed = new Vector3(Random.Range(10, 180), Random.Range(10, 180), Random.Range(10, 180));
    }

    void Update()
    {
        // rotate using the speeds stored in rotationSpeed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
