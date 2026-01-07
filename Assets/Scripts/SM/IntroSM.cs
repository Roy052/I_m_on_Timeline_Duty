using UnityEngine;

public class IntroSM : ShowSM
{
    private void Awake()
    {
        introSM = this;
        strings = DataManager.introStrs;
    }

    private void OnDestroy()
    {
        introSM = null;
    }
}
