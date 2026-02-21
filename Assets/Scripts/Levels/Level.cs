using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private GameObject cameraStartPosition;
    [SerializeField] private Transform landerTransformStart;
    [SerializeField] private float zoomedOutOrthoAmount;

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public Transform GetCameraStartPosition()
    {
        return cameraStartPosition.transform;
    }

    public Vector3 GetLanderTransformStart()
    {
        return landerTransformStart.position;
    }

    public float GetZoomedOutOrthoAmount()
    {
        return zoomedOutOrthoAmount;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}