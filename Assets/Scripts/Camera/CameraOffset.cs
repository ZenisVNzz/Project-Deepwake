using Unity.Cinemachine;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public static CameraOffset Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Move(float offset)
    {
        var cinemachine = Camera.main?.GetComponent<CinemachineBrain>();
        if (cinemachine != null)
        {
            var acticeCam = cinemachine.ActiveVirtualCamera as CinemachineCamera;
            if (acticeCam != null)
            {
                var component = acticeCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (component is CinemachinePositionComposer composer)
                {
                    composer.TargetOffset = new Vector3(0f, offset, 0f);
                }
            }
        }
    }
}
