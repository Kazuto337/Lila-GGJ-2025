using UnityEngine;
using UnityEngine.UI;

public class BrushSelector : MonoBehaviour
{
    [SerializeField] Sprite regularBubble;
    [SerializeField] Sprite explosiveBubble;
    [SerializeField] Sprite bubbleGum;

    [SerializeField] Image currentBubble;

    public void ChangeCurrentBrush(BubbleType bubbleType)
    {
        switch (bubbleType)
        {
            case BubbleType.Regular:
                currentBubble.sprite = regularBubble;
                break;
            case BubbleType.Explosive:
                currentBubble.sprite = explosiveBubble;
                break;
            case BubbleType.Gum:
                currentBubble.sprite = bubbleGum;
                break;
        }
    }

    public void ActivateCurrentBubbleImage()
    {
        currentBubble.gameObject.SetActive(true);
    }
    public void DesactivateCurrentBubbleImage()
    {
        currentBubble.gameObject.SetActive(false);
    }
}
