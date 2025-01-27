using UnityEngine;
using UnityEngine.Events;

public class MessageTriggerer : MonoBehaviour
{
    [SerializeField, TextArea] string message;

    [SerializeField] UnityEvent<string> OnMessageTriggered;

    private void OnEnable()
    {
        OnMessageTriggered.AddListener(GameManager.instance.MessageManager.PublishMessage);
    }

    public void TriggeredMessage()
    {
        OnMessageTriggered.Invoke(message);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggeredMessage();
            Destroy(gameObject); 
        }
    }

    private void OnDisable()
    {
        OnMessageTriggered.RemoveAllListeners();
    }
}
