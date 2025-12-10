using UnityEngine;
using UnityEngine.UIElements;

public class VFXTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            VFXSpawner.Spawn("Hit", pos);
        }
    }
}