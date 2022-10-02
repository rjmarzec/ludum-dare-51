using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndsGame : MonoBehaviour
{
    public void EndGame()
    {
        SceneManager.LoadScene("EndScreen");
    }
}
