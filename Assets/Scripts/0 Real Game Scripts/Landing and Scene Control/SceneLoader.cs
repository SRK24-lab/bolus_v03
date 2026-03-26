using UnityEngine.SceneManagement; // Add this line

using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Required for Coroutines

public class SceneLoader : MonoBehaviour
{
    // Method to load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Method to load the next scene in the build order
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }


}


