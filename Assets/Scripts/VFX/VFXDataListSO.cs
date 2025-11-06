using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VFXDataList", menuName = "GameData/VFX Data List")]
public class VFXDataListSO : ScriptableObject
{
    public List<VFXData> vfxList = new List<VFXData>();
}