using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PatrollStateChasing : StateMachineBehaviour
{
    float timer;
    public int patrollTimer = 10;
    public int chaseRadius = 5; // Distance at which chasing begins
    Transform target; // Will now reference object with tag "Princess"

    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the NavMeshAgent component
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0f;

        // Find the princess (instead of the player)
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }

        // Collect all patrol waypoints
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoints");
        wayPoints.Clear();
        foreach (GameObject go in gos)
        {
            wayPoints.Add(go.transform);
        }

        // Move to a random patrol point
        if (wayPoints.Count > 0)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.remainingDistance <= agent.stoppingDistance && wayPoints.Count > 0)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        timer += Time.deltaTime;
        if (timer > patrollTimer)
        {
            animator.SetBool("isPatrolling", false);
        }

        // Check if target (princess) is in chase radius
        if (target != null)
        {
            float distance = Vector3.Distance(target.position, animator.transform.position);
            if (distance < chaseRadius)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Stop agent movement when leaving patrol state
        if (agent != null)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
}