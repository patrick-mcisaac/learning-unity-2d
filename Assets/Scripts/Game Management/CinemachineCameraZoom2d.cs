using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2d : MonoBehaviour
{

    public const float NORMAL_ORTHOGRAPIC_SIZE = 10f;
    public static CinemachineCameraZoom2d Instance { get; private set; }

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }

    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetNormalOrthographicSize()
    {
        this.targetOrthographicSize = NORMAL_ORTHOGRAPIC_SIZE;
    }
}
