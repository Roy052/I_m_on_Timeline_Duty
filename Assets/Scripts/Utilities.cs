using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    private static readonly System.Random rng = new System.Random();

    public static void SetActive(this Component c, bool active)
    {
        c.gameObject.SetActive(active);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public static Vector2 GetLocalPosInCanvas(RectTransform rt, Canvas canvas)
    {
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rt.position);

        RectTransform cvRect = canvas.transform as RectTransform;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(cvRect, screen,
                                                                canvas.renderMode == RenderMode.ScreenSpaceOverlay
                                                                    ? null
                                                                    : canvas.worldCamera,
                                                                out Vector2 local);
        return local;
    }

    public static void SetSize(this RectTransform rt, float width, float height)
    {
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}
