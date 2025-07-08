using UnityEngine;

public class AttackStateChasing : StateMachineBehaviour
{
    Transform target;
    public float attackDistance = 2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null) return;

        animator.transform.LookAt(target);

        float distance = Vector3.Distance(target.position, animator.transform.position);

        // Nur wenn Ziel *zu weit weg ist*, zurück zu Chase
        if (distance > attackDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Kein Code nötig, aber kann später für Cleanup verwendet werden
    }
}