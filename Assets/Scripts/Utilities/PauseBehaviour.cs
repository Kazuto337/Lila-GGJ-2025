using UnityEngine;
using UnityEngine.InputSystem;

public class PauseBehaviour : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;

    [SerializeField] InputActionReference pauseAction;
    bool isPause;

    private void Update()
    {
        if (pauseAction.action.triggered)
        {
            isPause = !isPause;

            switch (isPause)
            {
                case true:
                    PauseGame();
                    break;
                case false:
                    UnPauseGame();
                    break;
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
