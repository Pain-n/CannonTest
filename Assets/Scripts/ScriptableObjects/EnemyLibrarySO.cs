using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyLibrary", menuName = "ScriptableObjects/EnemyLibrary")]
public class EnemyLibrarySO : ScriptableObject
{
    public List<EnemyLibrary> EnemyListsByDifficulty = new List<EnemyLibrary>();
}

[Serializable]
public class EnemyLibrary
{
    public int MinSpawnTimeDelay;
    public int MaxSpawnTimeDelay;
    public List<EnemyItem> EnemyList = new List<EnemyItem>();
}

[Serializable]
public class EnemyItem
{
    public int HP;
    public int Speed;
}
