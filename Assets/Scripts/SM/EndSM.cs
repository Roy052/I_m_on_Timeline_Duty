using UnityEngine;

public class EndSM : ShowSM
{
    private void Awake()
    {
        endSM = this;
        if (gm.isClear)
            strings = DataManager.gameEndStrs;
        else
            strings = DataManager.gameOverStrs;
    }

    private void OnDestroy()
    {
        endSM = null;
    }
}
