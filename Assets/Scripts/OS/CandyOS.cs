using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Animations;

[CreateAssetMenu]
public class CandyOS : ScriptableObject
{
    public List<CandyDataOS> candies;
}

[Serializable]
public class CandyDataOS
{
    [field: SerializeField]
    public Sprite sprite {get; private set;}
    [field: SerializeField]
    public CandyColor color {get; private set;}
    [field: SerializeField]
    public HitType hitType {get; private set;}
    [field: SerializeField]
    public RuntimeAnimatorController explodeController {get; private set;}
}
