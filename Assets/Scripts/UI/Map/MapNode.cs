using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MapNode : MonoBehaviour
{
    public MapNodeData nodeData;
    private Button button;
    public List<MapNode> linkedNode = new();
    public bool IsInteractable = true;
    public UnityAction<MapNode> OnNodeClicked;

    private Image OutlineImage;
    private Image IconImage;
    private SFXData ClickSFX = ResourceManager.Instance.GetAsset<SFXData>("UIButtonSFX");

    public void Init(MapNodeData nodeData)
    {
        this.nodeData = nodeData;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnPreSelect);
        OutlineImage = GetComponent<Image>();
        IconImage = transform.Find("Icon").GetComponent<Image>();
        IconImage.sprite = nodeData.nodeType.image;
    }

    public void OnPreSelect()
    {
        if (IsInteractable)
        {
            SFXManager.Instance.Play(ClickSFX, transform.position);
            OnNodeClicked?.Invoke(this);         
        }    
    }

    public void OnSelect()
    {    
        nodeData.OnSelect();
    }

    public void SetInteractable(bool value)
    {
        IsInteractable = value;

        if (button != null)
            button.interactable = value;

        if (IconImage != null)
            IconImage.color = value ? Color.white : Color.gray;
    }

    public void MarkAsCurrent(bool value)
    {
        if (OutlineImage != null)
            OutlineImage.color = value ? Color.cyan : (IsInteractable ? Color.white : Color.gray);
    }

    public void MaskAsSelected(bool value)
    {
        if (OutlineImage != null)
            OutlineImage.color = value ? Color.orange : (IsInteractable ? Color.white : Color.gray);
    }
}
