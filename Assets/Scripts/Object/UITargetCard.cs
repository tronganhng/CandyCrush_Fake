using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UITargetCard: MonoBehaviour
{
    public TargetStat stat;
    [SerializeField] private Image image;
    [SerializeField] private Text amountTxt;
    [SerializeField] private CandyOS[] dataOs;

    private void OnEnable()
    {
        Controller.Instance.OnCandyMatched += UpdateText;
    }
    private void OnDisable()
    {
        Controller.Instance.OnCandyMatched -= UpdateText;
    }

    public void SetInfo(TargetStat sample)
    {
        stat = sample;
        if(sample.color == CandyColor.RainBow) image.sprite = dataOs[(int)HitType.ColorBomb].candies[0].sprite;
        else image.sprite = dataOs[(int)sample.hitType].candies[(int)sample.color].sprite;
        amountTxt.text = sample.amount.ToString();
    }

    public void SetAmount(int subtrahend)
    {
        stat.amount -= subtrahend;
        if(stat.amount < 0) stat.amount = 0;
    }

    public void UpdateText()
    {
        amountTxt.text = stat.amount.ToString();
    }
}