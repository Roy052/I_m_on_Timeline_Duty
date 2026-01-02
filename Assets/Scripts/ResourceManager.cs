using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : Singleton
{
    public Sprite[] cardSprites;
    public Sprite[] heartSprites;

    public Material matGrayScale;

    private void Awake()
    {
        if (resourceManager != null)
            return;

        resourceManager = this;
    }

    public Sprite GetHeartSprite(int value)
    {
        return heartSprites[value + 4];
    }
}
