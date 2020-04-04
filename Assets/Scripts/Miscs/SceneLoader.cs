using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] float timer = 3.0f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // load next scenes
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
