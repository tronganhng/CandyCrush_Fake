using System;
using System.Collections.Generic;
using UnityEngine;

public static class MenuEvent
{
    public static Action<LevelDataList> OnLoadLevelNodes;
    public static Action<LevelData> OnEnterLevel;
}