using UnityEngine;
using UnityEngine.AI;

public class IdleStateChasing : StateMachineBehaviour
{
    float timer;
    public int idleTimer = 5;
    public int chaseRadius = 5; // distance Radius
    Transform target;
    NavMeshAgent agent;

    // Wird beim Eintritt in den Idle-State aufgerufen
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }

        agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath(); // kill any residual movement
        }
    }

    // Wird bei jedem Frame im Idle-State ausgeführt
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer > idleTimer)
        {
            animator.SetBool("isPatrolling", true);
        }

        if (target == null) return;

        float distance = Vector3.Distance(target.position, animator.transform.position);
        if (distance < chaseRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // Wird beim Verlassen des Idle-States aufgerufen
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent != null)
        {
            agent.isStopped = false; // Bewegung wieder erlauben
        }
    }
}