using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    [SerializeField] TMP_Text m_Text;

    public void PublishMessage(string message)
    {
        m_Text.text = message;

        StartCoroutine(ResetTextBox(4));
    }

    IEnumerator ResetTextBox(float duration)
    {
        yield return new WaitForSeconds(duration);

        m_Text.text = string.Empty;
    }
}
