using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public int scoreValue = 1;              // How many points this heart gives
    public float rotateSpeed = 90f;         // Rotation speed in degrees per second
    public float floatSpeed = 0.5f;         // Vertical floating speed
    public float floatHeight = 0.25f;       // Distance to float up and down
    public float lifetime = 10f;            // Time before auto-destroy

    private Vector3 startPos;               // Initial position for floating
    private float floatTimer;

    void Start()
    {
        // Store the starting position
        startPos = transform.position;

        // Destroy the heart after 10 seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Rotate the heart around the Y-axis
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);

        // Make the heart float up and down
        floatTimer += Time.deltaTime * floatSpeed;
        float yOffset = Mathf.Sin(floatTimer) * floatHeight;
        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the player collides with the heart
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(scoreValue);
            Destroy(gameObject);
        }
    }
}