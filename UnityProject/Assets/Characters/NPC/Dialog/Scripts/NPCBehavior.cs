using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public Animator animator;
    public Transform targetPoint;
    public float speed = 2f;
    public float stopDistance = 1.2f;

    private bool isWalking = false;
    private bool hasArrived = false;

    void Start()
    {
        // NPC starts hidden until triggered
        gameObject.SetActive(false);
    }

    public void SpawnNPC()
    {
        // Activate NPC and start walking
        gameObject.SetActive(true);
        isWalking = true;
        hasArrived = false;

        animator.SetBool("isWalking", true);   // play walking animation
        animator.SetBool("isWaving", false);   // make sure waving is off
    }

    void Update()
    {
        // If NPC should walk and hasn't arrived yet
        if (isWalking && !hasArrived)
        {
            // Move toward the target point
            Vector3 direction = (targetPoint.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Smoothly rotate toward the target
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Check if NPC is close enough to stop
            float distance = Vector3.Distance(transform.position, targetPoint.position);
            if (distance < stopDistance)
            {
                ArriveAndWave(); // Stop walking and start waving
            }
        }
    }

    void ArriveAndWave()
    {
        // Only trigger once
        if (hasArrived) return;

        isWalking = false;
        hasArrived = true;

        animator.SetBool("isWalking", false);  // stop walking animation
        animator.SetBool("isWaving", true);    // play waving animation
    }
}
