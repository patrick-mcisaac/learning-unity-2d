using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private int points = 5;

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public int GetPoints()
    {
        return points;
    }
}
