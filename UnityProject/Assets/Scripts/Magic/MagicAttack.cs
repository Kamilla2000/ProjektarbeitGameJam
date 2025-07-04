using UnityEngine;
using UnityEngine.InputSystem;

public class MagicAttack : MonoBehaviour
{
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform castPoint; // The point on the hand where magic spawns
    [SerializeField] private float magicSpeed = 10f; // Speed of the magic projectile (reduced for smoothness)
    [SerializeField] private float cooldown = 1f; // Cooldown time between casts

    private float lastCastTime;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        // Check if Q key was pressed this frame and cooldown is over
        if (Keyboard.current.qKey.wasPressedThisFrame && Time.time >= lastCastTime + cooldown)
        {
            CastMagic();
            lastCastTime = Time.time; // Update last cast time
        }
    }

    void CastMagic()
    {
        animator.SetTrigger("Cast"); // Trigger the cast animation

        // Instantiate the magic prefab at the castPoint position and rotation
        GameObject magic = Instantiate(magicPrefab, castPoint.position, castPoint.rotation);

        Rigidbody rb = magic.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Set the velocity to move forward from the castPoint's local forward direction
            rb.linearVelocity = castPoint.forward * magicSpeed;
        }

        Destroy(magic, 5f); // Destroy the magic after 5 seconds to clean up
        Debug.Log($"Magic spawned at position: {magic.transform.position}, velocity: {rb.linearVelocity}");
    }
}
