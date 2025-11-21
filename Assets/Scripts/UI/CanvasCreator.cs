using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCreator
{
    public GameObject Create(bool WorldSpace)
    {
        GameObject CanvasGO = new GameObject("Canvas");
        Canvas canvas = CanvasGO.AddComponent<Canvas>();

        if (WorldSpace)
        {
            canvas.renderMode = RenderMode.WorldSpace;
            RectTransform rectTransform = CanvasGO.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(800, 600);
            rectTransform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
            canvas.sortingOrder = 100;
        }
        else
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 99;
        }
          
        CanvasScaler scaler = CanvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1;
        scaler.referencePixelsPerUnit = 100;

        CanvasGO.AddComponent<GraphicRaycaster>();

        return CanvasGO;
    }
}
