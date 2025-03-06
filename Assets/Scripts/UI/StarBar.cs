using UnityEngine;
using UnityEngine.UI;

public class StarBar : MonoBehaviour
{
    public Image fillBar;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RectTransform[] stars;

    public void CheckFill()
    {
        foreach (RectTransform item in stars)
        {
            if (fillBar.fillAmount >= item.anchoredPosition.x / bar.rect.width)
            {
                item.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                item.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public int GetActiveStar()
    {
        int result = 0;
        foreach (RectTransform item in stars)
        {
            if (item.gameObject.activeSelf) result++;
        }
        return result;
    }
}