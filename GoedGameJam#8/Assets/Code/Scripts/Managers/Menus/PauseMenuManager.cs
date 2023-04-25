using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static bool paused;
    void Start()
    {
        
    }

    private void Update()
    {
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Resume(string sceneName)
    {
        
        SceneManager.UnloadSceneAsync(sceneName);
        InputManager.pmEnabled = false;
        paused = false;
    }

    public void MainMenu(string sceneName)
    {
        paused = false;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    public void Settings(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }


    public void Quit()
    {
        Application.Quit();
    }



}

   