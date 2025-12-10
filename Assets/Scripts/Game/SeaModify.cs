using UnityEngine;
using UnityEngine.Tilemaps;

public class SeaModify : MonoBehaviour
{
    public static SeaModify Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ChangeSeaColor(Color newColor)
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        if (tilemap != null)
        {
            tilemap.color = newColor;
        }
    }
}
