using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerTransformStart;
    [SerializeField] private GameObject startCameraTarget;
    [SerializeField] private float zoomedOutOrthographicAmount;


    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public Vector3 GetLanderStartPosition()
    {
        return landerTransformStart.position;
    }

    public Transform GetCameraTargetStartTransform()
    {
        return startCameraTarget.transform;
    }
    public float GetZoomedOutOrthographicAmount()
    {
        return zoomedOutOrthographicAmount;
    }
}
