using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    // Close pause menu - unpause
    public void ResumeGame(string pauseMenuSceneName)
    {
        SceneManager.UnloadSceneAsync(pauseMenuSceneName);
        UnPauseGame();
    }
    // Open Main menu - unpause
    public void OpenMainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        UnPauseGame();
    }
    // Open Settings menu
    public void OpenSettings(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    // Close Application
    public void QuitGame()
    {
        Application.Quit();
    }

    private void UnPauseGame() {
        Time.timeScale = 1f;
        return;
    }
}

   