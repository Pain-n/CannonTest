using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "ScriptableObjects/UpgradeList")]
public class UpgradeListSO : ScriptableObject
{
    public List<UpgradeListItem> UpgradeList = new List<UpgradeListItem>();

    public object Clone()
    {
        return MemberwiseClone();
    }
}

[Serializable]
public class UpgradeListItem
{
    public UpgradeType UpgradeType;
    [HideInInspector]
    public int CurrentLvl;
    public List<UpgradeStage> UpgradeStages = new List<UpgradeStage>();
}

[Serializable]
public class UpgradeStage
{
    public float Value;
    public int Cost;
}

public enum UpgradeType
{
    AttackSpeed,
    Damage
}
