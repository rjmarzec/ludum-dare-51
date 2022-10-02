using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PanelFadesOut : MonoBehaviour
{
    public AudioClip ggSound;

    private UnityEngine.UI.Image image;
    private float timer;
    public float timeToFadeOver = 1;

    void Start()
    {
        AudioSource.PlayClipAtPoint(ggSound, Vector3.zero);

        timer = 0;
        image = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        image.color = new Color(0, 0, 0, 1 - (timer/timeToFadeOver));

        if (timer > timeToFadeOver)
        {
            Destroy(gameObject);
        }
    }
}
