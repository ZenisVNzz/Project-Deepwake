using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "AssetReferences", menuName = "DataSystem/AssetReferences")]
public class AssetReferences : ScriptableObject
{
    [SerializeField] private string _key;
    [SerializeField] private List<AssetReference> _assets;
    [SerializeField] private List<AssetLabelReference> _labels;

    public string Key => _key;
    public List<AssetReference> Assets => _assets;
    public List<AssetLabelReference> Labels => _labels;
}
