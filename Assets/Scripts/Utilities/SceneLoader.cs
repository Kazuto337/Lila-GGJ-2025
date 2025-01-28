using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
