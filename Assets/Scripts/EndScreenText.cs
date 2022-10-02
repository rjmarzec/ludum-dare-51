using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenText : MonoBehaviour
{
    private TextMeshProUGUI tmp;

    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        tmp.text = "You helped create\n" + DoesStuffEvery10Seconds.planetCount + " planets.\n\nNice job!";
    }
}
