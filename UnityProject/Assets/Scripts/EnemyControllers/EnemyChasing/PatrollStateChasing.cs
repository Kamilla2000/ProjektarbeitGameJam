using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PatrollStateChasing : StateMachineBehaviour
{
    public float patrolDuration = 10f;
    public float chaseRadius = 5f;

    private float timer;
    private Transform target;
    private NavMeshAgent agent;
    private List<Transform> wayPoints = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0f;

        // Find princess
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }

        // Find waypoints
        wayPoints.Clear();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoints");
        foreach (GameObject go in gos)
        {
            wayPoints.Add(go.transform);
        }

        // Go to a random waypoint
        if (wayPoints.Count > 0)
        {
            agent.SetDestination(GetClosestWaypoint(animator.transform.position).position);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wayPoints.Count == 0) return;

        timer += Time.deltaTime;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

        if (timer > patrolDuration)
        {
            animator.SetBool("isPatrolling", false);
        }

        // Chase if within range
        if (target != null)
        {
            float distance = Vector3.Distance(animator.transform.position, target.position);
            if (distance < chaseRadius)
            {
                animator.SetBool("isChasing", true);
            }
        }
    }

    Transform GetClosestWaypoint(Vector3 fromPos)
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform t in wayPoints)
        {
            float dist = Vector3.Distance(fromPos, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
            agent.ResetPath();
    }
}