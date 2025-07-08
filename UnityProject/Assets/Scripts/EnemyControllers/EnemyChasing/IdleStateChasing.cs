using UnityEngine;

public class IdleStateChasing : StateMachineBehaviour
{
    float timer;
    public int idleTimer = 5;
    public int chaseRadius = 5; // distance Radius
    Transform target;

    // Wird beim Eintritt in den Idle-State aufgerufen
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
        {
            target = princessObj.transform;
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
}