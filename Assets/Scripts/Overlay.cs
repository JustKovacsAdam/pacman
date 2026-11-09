using UnityEngine;
using UnityEngine.UI;

// Builds simple full-screen UI overlays (game over, pause, ...) from code,
// so no extra scene/prefab editing is needed.
public static class Overlay
{
    public static GameObject Show(string title, Color titleColor, string body)
    {
        GameObject root = new GameObject("Overlay", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler));
        Canvas canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 1f;

        GameObject background = new GameObject("Background", typeof(RectTransform), typeof(Image));
        background.transform.SetParent(root.transform, false);
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
        background.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);

        AddText(root.transform, "Title", title, 120, titleColor, new Vector2(0, 140));
        AddText(root.transform, "Body", body, 42, Color.white, new Vector2(0, -110));
        return root;
    }

    public static Text AddText(Transform parent, string name, string content, int size, Color color, Vector2 position)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(1800, 400);
        rect.anchoredPosition = position;

        Text text = go.GetComponent<Text>();
        text.font = DefaultFont();
        text.text = content;
        text.fontSize = size;
        text.color = color;
        text.alignment = TextAnchor.MiddleCenter;
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;
        return text;
    }

    static Font cachedFont;

    static Font DefaultFont()
    {
        if (cachedFont != null)
        {
            return cachedFont;
        }
        // Reuse the font of the existing HUD text (built-in Arial) if there is one.
        Text existing = Object.FindObjectOfType<Text>();
        if (existing != null && existing.font != null)
        {
            cachedFont = existing.font;
        }
        else
        {
            cachedFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }
        return cachedFont;
    }
}
