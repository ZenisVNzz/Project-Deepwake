using Unity.Cinemachine;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class SyncCamera: MonoBehaviour
{
    [SerializeField] private CinemachineCamera virtualCam;

    private Camera mirrorCam;
    private Camera mainCam;

    void Start()
    {
        mirrorCam = GetComponent<Camera>();
        mainCam = Camera.main;

    }

    void LateUpdate()
    {
        if (virtualCam == null) return;

        virtualCam.Lens.OrthographicSize = mainCam.orthographicSize;
    }
}
