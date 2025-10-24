using UnityEngine;

[System.Serializable]
public class CharacterAttributes
{
    public int VIT; 
    public int DEF; 
    public int STR; 
    public int LUCK; 
    public int AvailablePoints = 10;

    public void AddPoint(string attr)
    {
        if (AvailablePoints <= 0) return;

        switch (attr)
        {
            case "VIT": VIT++; break;
            case "DEF": DEF++; break;
            case "STR": STR++; break;
            case "LUCK": LUCK++; break;
        }

        AvailablePoints--;
    }
}