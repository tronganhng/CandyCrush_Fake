using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Candy : MonoBehaviour
{
    public CandyColor color;
    public HitType hitType;
    public Vector2Int matrixPos;
    public SpriteRenderer spriteRenderer;
    public RuntimeAnimatorController explodeController;
    [SerializeField] protected float explodeLifetime = .3f;
    [SerializeField] protected float fallTime = 1f;

    public virtual void SetInfo(CandyDataOS sample)
    {
        name = sample.color.ToString() + hitType.ToString();
        spriteRenderer.sprite = sample.sprite;
        explodeController = sample.explodeController;
        color = sample.color;
        hitType = sample.hitType;
    }

    public void Explode()
    {
        GameObject eff = EffecPool.Instance.GetExplodeEffAt(transform.position);
        eff.GetComponent<CandyExplodeEff>().SetClip(explodeController, explodeLifetime);
        gameObject.SetActive(false);
    }

    public void Move(Vector2Int targetPos)
    {
        Controller.Instance.candyMoving = true;
        transform.DOLocalMove((Vector2)targetPos, fallTime).SetEase(Ease.OutQuad)
        .OnComplete(() => Controller.Instance.candyMoving = false);
    }
}