using UnityEngine;
using System.Collections.Generic;

public class EndSM : ShowSM
{
    private void Awake()
    {
        endSM = this;
        if (gm.gameResult.isClear)
            strings = new List<string>(DataManager.gameEndStrs);
        else
            strings = new List<string>(DataManager.gameOverStrs);

        strings.Insert(0, DataManager.GetString("Ending_Result", gm.gameResult.countAnomaly.ToString(), gm.gameResult.countFix.ToString()));
    }

    private void OnDestroy()
    {
        endSM = null;
    }
}
