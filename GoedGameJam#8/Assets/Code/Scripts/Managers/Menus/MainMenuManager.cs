using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SingleSceneTransition(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void AdditiveSceneTransition(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
