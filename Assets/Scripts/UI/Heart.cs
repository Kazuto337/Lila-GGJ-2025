using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    public void EmptyHeart()
    {
        image.sprite = emptyHeart;
    }

    public void FillHeart()
    {
        image.sprite = fullHeart;
    }
}
