using System.Collections;
using UnityEngine;
using UnityEditor.Animations;

public class CandyExplodeEff : MonoBehaviour
{
    public Animator ani;
    public void SetClip(RuntimeAnimatorController controller, float lifeTime)
    {
        ani.runtimeAnimatorController = controller;
        StartCoroutine(TurnOff(lifeTime));
    }

    IEnumerator TurnOff(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }
}