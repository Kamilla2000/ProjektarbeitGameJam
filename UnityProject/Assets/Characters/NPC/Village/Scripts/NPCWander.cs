using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCWander : MonoBehaviour
{
    public Transform[] wanderPoints; // Points around the village where the NPC can walk
    public float pauseDuration = 3f; // Time the NPC waits at each point

    private NavMeshAgent agent;
    private Animator animator;
    private int lastIndex = -1; // To avoid repeating the same point
    private bool isWaiting = false; // Prevent multiple coroutines

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GoToNextPoint(); // Start walking right away
    }

    void Update()
    {
        // Check if NPC reached the destination and isn't already waiting
        if (!isWaiting && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(PauseBeforeNextMove());
        }
    }

    IEnumerator PauseBeforeNextMove()
    {
        isWaiting = true;

        agent.isStopped = true; // Stop movement
        animator.SetTrigger("lookAround"); // Play look around animation

        yield return new WaitForSeconds(pauseDuration); // Wait a bit

        agent.isStopped = false;
        GoToNextPoint(); // Move to the next point

        isWaiting = false;
    }

    void GoToNextPoint()
    {
        if (wanderPoints.Length == 0) return;

        int nextIndex = lastIndex;

        // Only one point = no choice
        if (wanderPoints.Length == 1)
        {
            agent.SetDestination(wanderPoints[0].position);
            return;
        }

        // Make sure the new point isn't the same as the last one
        while (nextIndex == lastIndex)
        {
            nextIndex = Random.Range(0, wanderPoints.Length);
        }

        lastIndex = nextIndex;
        agent.SetDestination(wanderPoints[nextIndex].position);
    }

}