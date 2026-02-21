using System;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{

    public static CinemachineCameraZoom2D Instance;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private CameraTarget lander;

    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    // private float startOrthoSize = 15f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Target = lander;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, NORMAL_ORTHOGRAPHIC_SIZE, zoomSpeed * Time.deltaTime);
    }

    public void SetCameraPosition(Transform target)
    {
        cinemachineCamera.Target.TrackingTarget = target;
    }

    public void SetCameraOrthoAmount(float amount)
    {
        cinemachineCamera.Lens.OrthographicSize = amount;
    }




}