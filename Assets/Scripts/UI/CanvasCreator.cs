using Mirror.Examples.Basic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCreator
{
    public GameObject Create()
    {
        GameObject CanvasGO = new GameObject("Canvas");
        Canvas canvas = CanvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 5;

        CanvasScaler scaler = CanvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1;
        scaler.referencePixelsPerUnit = 100;

        CanvasGO.AddComponent<GraphicRaycaster>();

        return CanvasGO;
    }
}
