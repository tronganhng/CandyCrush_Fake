using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    public int iD;
    public int levelNumber;
    public TargetStat[] targets;
    public int totalTurn;
    public int starCnt;
}

public class LevelDataList
{
    public int currentLevel;
    public List<LevelData> levelList;
}