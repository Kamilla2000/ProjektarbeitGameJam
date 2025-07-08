using UnityEngine;

public class AttackStateChasing : StateMachineBehaviour
{
    Transform target;
    public float attackDistance = 2f;

    // Wird beim Eintritt in den State ausgeführt
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
        }
    }

    // Wird jede Frame im Attack-State aufgerufen
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (target == null) return;

        // NPC soll sich zur Princess drehen
        animator.transform.LookAt(target);

        float distance = Vector3.Distance(target.position, animator.transform.position);
        if (distance < attackDistance)
        {
            // In deiner Logik kann das ggf. bedeuten, dass er *nicht mehr angreift* – je nach Animator-Setup
            animator.SetBool("isAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hier musst du nichts machen, außer du willst z.B. eine Animation stoppen
    }
}