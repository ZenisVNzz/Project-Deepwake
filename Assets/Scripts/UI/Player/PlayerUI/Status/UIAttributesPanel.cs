using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAttributesPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text vitText, defText, strText, luckText, pointsText;
    [SerializeField] private Button vitPlus, defPlus, strPlus, luckPlus;

    private CharacterAttributes attributes;

    public event Action<string> OnAddPointRequested;

    public void Bind(CharacterAttributes attr)
    {
        attributes = attr;
        Refresh();
    }

    private void Refresh()
    {
        if (attributes == null) return;
        vitText.text = attributes.VIT.ToString();
        defText.text = attributes.DEF.ToString();
        strText.text = attributes.STR.ToString();
        luckText.text = attributes.LUCK.ToString();
        pointsText.text = attributes.AvailablePoints.ToString();

        bool canAdd = attributes.AvailablePoints > 0;
        vitPlus.interactable = canAdd;
        defPlus.interactable = canAdd;
        strPlus.interactable = canAdd;
        luckPlus.interactable = canAdd;
    }

    private void OnEnable()
    {
        vitPlus.onClick.AddListener(() => AddPoint("VIT"));
        defPlus.onClick.AddListener(() => AddPoint("DEF"));
        strPlus.onClick.AddListener(() => AddPoint("STR"));
        luckPlus.onClick.AddListener(() => AddPoint("LUCK"));
    }

    private void OnDisable()
    {
        vitPlus.onClick.RemoveAllListeners();
        defPlus.onClick.RemoveAllListeners();
        strPlus.onClick.RemoveAllListeners();
        luckPlus.onClick.RemoveAllListeners();
    }

    private void AddPoint(string attr)
    {
        OnAddPointRequested?.Invoke(attr);
        Refresh();
    }
}
