using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIAttributesPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text vitText, defText, strText, luckText, pointsText;
    [SerializeField] private Button vitPlus, defPlus, strPlus, luckPlus;

    private CharacterAttributes attributes;

    public void Bind(CharacterAttributes attr)
    {
        attributes = attr;
        Refresh();
    }

    private void Refresh()
    {
        vitText.text = attributes.VIT.ToString();
        defText.text = attributes.DEF.ToString();
        strText.text = attributes.STR.ToString();
        luckText.text = attributes.LUCK.ToString();
        pointsText.text = attributes.AvailablePoints.ToString();
    }

    private void OnEnable()
    {
        vitPlus.onClick.AddListener(() => AddPoint("VIT"));
        defPlus.onClick.AddListener(() => AddPoint("DEF"));
        strPlus.onClick.AddListener(() => AddPoint("STR"));
        luckPlus.onClick.AddListener(() => AddPoint("LUCK"));
    }

    private void AddPoint(string attr)
    {
        attributes.AddPoint(attr);
        Refresh();
    }
}
