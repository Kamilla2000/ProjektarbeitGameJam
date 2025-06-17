using UnityEngine;

public class CoinScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddCoin(1);
            Destroy(gameObject); // Coin verschwindet
        }
    }
}