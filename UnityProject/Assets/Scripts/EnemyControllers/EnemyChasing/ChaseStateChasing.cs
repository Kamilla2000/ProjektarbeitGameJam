using UnityEngine;
using UnityEngine.AI;

public class ChaseStateChasing : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform target;
    public int minChaseRadius = 5;
    public int maxChaseRadius = 15;
    public float attackDistance = 2f;

    // Wird beim Eintritt in den State ausgeführt
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }
    }

    // Wird jede Frame im State aufgerufen
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null) return;

        agent.SetDestination(target.position);

        float distance = Vector3.Distance(target.position, animator.transform.position);

        if (distance > maxChaseRadius)
        {
            animator.SetBool("isChasing", false);
        }
        else if (distance < attackDistance)
        {
            animator.SetBool("isAttacking", true);
        }
    }

    // Wird beim Verlassen des States ausgeführt
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }
}