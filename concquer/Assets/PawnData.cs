using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PawnData
{
    public PawnData(int cost, int upkeep)
    {
        Cost = cost;
        Upkeep = upkeep;
    }

    [SerializeField]
    public string TypeName;

    [SerializeField]
    public Sprite SpriteImage;

    [SerializeField]
    public PawnType Type { get; private set; }

    [SerializeField]
    private int Cost;

    [SerializeField]
    private int Upkeep;
}
