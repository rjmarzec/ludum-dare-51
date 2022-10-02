using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoesToMainScreenOnClick : MonoBehaviour
{
    public AudioClip startSound;

    public void GoesToMainScreen()
    {
        AudioSource.PlayClipAtPoint(startSound, Vector3.zero);
        SceneManager.LoadScene("MainScene");
    }
}
