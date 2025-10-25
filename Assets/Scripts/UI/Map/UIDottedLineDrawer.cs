using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIDottedLineDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform lineParent;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing = 10f;

    private List<List<GameObject>> allDots = new();

    public void DrawDottedLine(RectTransform start, RectTransform end)
    {
        List<GameObject> dots = new List<GameObject>();

        Vector2 startPos = start.anchoredPosition;
        Vector2 endPos = end.anchoredPosition;

        Vector2 dir = (endPos - startPos).normalized;
        float distance = Vector2.Distance(startPos, endPos);
        int dotCount = Mathf.FloorToInt(distance / dotSpacing);

        for (int i = 0; i <= dotCount; i++)
        {
            Vector2 pos = startPos + dir * (i * dotSpacing);
            GameObject dot = Instantiate(dotPrefab, lineParent);
            dot.GetComponent<RectTransform>().anchoredPosition = pos;
            dots.Add(dot);
        }

        allDots.Add(dots);
    }

    public void ClearAllDots()
    {
        foreach (var line in allDots)
            foreach (var dot in line)
                Destroy(dot);
        allDots.Clear();
    }
}
