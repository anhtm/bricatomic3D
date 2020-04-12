using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Load next scene with a timer
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] float timer = 3.0f;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
