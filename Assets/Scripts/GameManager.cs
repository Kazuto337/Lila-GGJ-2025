using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] MessageManager messageManager;

    public MessageManager MessageManager { get => messageManager; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }
    private void Start()
    {
        Introduction();
    }

    private void Introduction()
    {
        string text1 = "Use [W A S D] to move";
        string text2 = "Press the [SPACEBAR] to jump and reach new heights with the double jump";
        string text3 = "Our goal is to find the three magical bubbles and use their powers to tackle multiple situations. Get ready to explore, challenge your skills, and awaken the magic in this amazing journey.";

        string[] introMessages= {text1 , text2 , text3 };

        StartCoroutine(IntroductionMessages(introMessages));
    }

    private IEnumerator IntroductionMessages(string[] messages)
    {
        foreach (string message in messages)
        {
            messageManager.PublishMessage(message);
            yield return new WaitForSeconds(7);
        }
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(3);
    }
    
    public void EndGame()
    {
        SceneManager.LoadScene(2);
    }

    public void Return2MainMenu()
    {
        SceneManager.LoadScene(2);
    }
}
