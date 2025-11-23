using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXDataList", menuName = "GameData/SFX Data List")]
public class SFXDataListSO : ScriptableObject
{
    public List<SFXData> list = new List<SFXData>();
}