using System;
using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    [SerializeField] private new CinemachineCamera camera;
    [SerializeField] private CameraTarget startTarget;
    [SerializeField] private CameraTarget lander;

    private const float NORMAL_ORTHOGRAPHIC_SIZE = 10f;

    [SerializeField] private float startOrthoSize;

    private void Start()
    {
        camera.Target = startTarget;
        camera.Lens.OrthographicSize = startOrthoSize;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        camera.Target = lander;
        camera.Lens.OrthographicSize = Mathf.Lerp(camera.Lens.OrthographicSize, NORMAL_ORTHOGRAPHIC_SIZE, zoomSpeed * Time.deltaTime);
    }


}