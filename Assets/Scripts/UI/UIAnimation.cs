using UnityEngine;
using DG.Tweening;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private UIAnim onEnable;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    Vector3 startPos;
    private enum UIAnim
    {
        FadeIn,
        FadeOut,
        PopIn
    }
    private void Start()
    {
        startPos = rectTransform.transform.localPosition;
    }
    private void OnEnable()
    {
        switch (onEnable)
        {
            case UIAnim.FadeIn:
                FadeIn();
                break;
            case UIAnim.PopIn:
                PopIn();
                break;
            default:
                break;
        }
    }

    private void OnDisable()
    {
        rectTransform.localPosition = startPos;
        DOTween.Kill(rectTransform);
    }

    private void FadeIn()
    {
        canvasGroup.alpha = 0f;
        rectTransform.transform.localPosition = rectTransform.transform.localPosition + new Vector3(0, 500, 0);
        Vector3 targetPos = rectTransform.transform.localPosition - new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(targetPos, 0.7f, false).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1, .35f);
    }

    private void PopIn()
    {
        rectTransform.transform.localScale = Vector3.zero;
        rectTransform.DOScale(1, 1).SetEase(Ease.OutBack);
        rectTransform.DOAnchorPosY(15f, 1f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);

    }
}