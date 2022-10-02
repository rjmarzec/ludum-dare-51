using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraDisplay : MonoBehaviour
{
    void Start()
    {
        GetComponent<Camera>().targetDisplay = -1;
    }
}
