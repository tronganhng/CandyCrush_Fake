using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int levelNumber;
    public TargetStat[] targets;
    public bool isComplete;
    public int starCnt; 
}

public class LevelCollection
{
    public List<LevelData> levelList;
}