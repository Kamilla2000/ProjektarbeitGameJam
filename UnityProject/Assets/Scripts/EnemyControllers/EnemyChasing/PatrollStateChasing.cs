using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PatrollStateChasing : StateMachineBehaviour
{
    float timer;
    public int patrollTimer = 10;
    public int chaseRadius = 5;
    Transform target;

    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        timer = 0f;

        // Princess finden
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }

        // Vorhandene WayPoints finden
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoints");
        wayPoints.Clear();

        if (gos.Length == 0)
        {
            // 🔥 Keine Waypoints gefunden → automatisch generieren
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomPoint = animator.transform.position + new Vector3(
                    Random.Range(-10f, 10f),
                    0f,
                    Random.Range(-10f, 10f)
                );

                // Prüfen, ob Punkt begehbar (optional!)
                if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    GameObject wp = new GameObject("AutoWaypoint_" + i);
                    wp.transform.position = hit.position;
                    wp.tag = "WayPoints";
                    wayPoints.Add(wp.transform);
                }
            }
        }
        else
        {
            foreach (GameObject go in gos)
            {
                wayPoints.Add(go.transform);
            }
        }

        // Zu erstem Punkt gehen
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
        if (agent != null)
        {
            agent.SetDestination(agent.transform.position);
        }
    }
}