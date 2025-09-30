using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetReferencesList", menuName = "DataSystem/AssetReferencesList")]
public class AssetReferencesList : ScriptableObject
{
    [SerializeField] private List<AssetReferences> _references;
    public List<AssetReferences> References => _references; 
}
