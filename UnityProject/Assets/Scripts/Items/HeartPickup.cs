using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int scoreValue = 1;              // How many points this heart gives
    public float rotateSpeed = 90f;         // Rotation speed in degrees per second
    public float floatSpeed = 0.5f;         // Speed of vertical floating
    public float floatHeight = 0.25f;       // Vertical range of floating
    public float lifetime = 10f;            // Time before auto-destroy

    private Vector3 basePosition;           // Starting position for floating
    private float floatTimer;

    void Start()
    {
        // Store the original spawn position
        basePosition = transform.position;

        // Auto-destroy after a set lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Rotate around Y-axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        // Float up and down using sine wave
        floatTimer += Time.deltaTime * floatSpeed;
        float yOffset = Mathf.Sin(floatTimer) * floatHeight;

        // Apply floating position
        transform.position = new Vector3(basePosition.x, basePosition.y + yOffset, basePosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If player collects the heart
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}
