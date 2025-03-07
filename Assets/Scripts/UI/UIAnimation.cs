using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private UIAnim onEnable;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    Vector3 startPos;
    private enum UIAnim
    {
        FadeIn,
        DropDown,
        PopIn
    }
    private void Awake()
    {
        startPos = rectTransform.localPosition;
    }
    private void OnEnable()
    {
        switch (onEnable)
        {
            case UIAnim.FadeIn:
                FadeIn();
                break;
            case UIAnim.DropDown:
                DropDown();
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
        if (canvasGroup == null)
        {
            Debug.Log("You need canvasGroup!!!");
            return;
        }
        canvasGroup.alpha = 0f;
        rectTransform.localPosition = rectTransform.localPosition + new Vector3(0, 500, 0);
        Vector3 targetPos = rectTransform.localPosition - new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(targetPos, 0.7f, false).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1, .35f);
    }

    private void PopIn()
    {
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(1, 1).SetEase(Ease.OutBack);
        rectTransform.DOAnchorPosY(15f, 1f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);

    }

    private void DropDown()
    {
        rectTransform.localPosition = rectTransform.localPosition + new Vector3(0, 500, 0);
        Vector3 targetPos = rectTransform.localPosition - new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(targetPos, 0.7f, false).SetEase(Ease.OutBack).OnComplete(() => rectTransform.DOAnchorPosY(15f, 1f)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine));
    }

    public void RiseUp()
    {
        Vector3 targetPos = rectTransform.localPosition + new Vector3(0, 1000, 0);
        rectTransform.DOAnchorPos(targetPos, 0.4f, false).SetEase(Ease.InBack).OnComplete(() => transform.parent.gameObject.SetActive(false));
    }
}