using System;
using UnityEngine;

[Serializable]
public class NPCIdleState : BaseState
{
    public float MinWaitTime;
    public float MaxWaitTime;

    private float leaveTime;

    public override void OnEnterState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;
        npcController.SetDestionation(npcController.transform.position);

        leaveTime = Time.time + UnityEngine.Random.Range(MinWaitTime, MaxWaitTime);
    }

    public override void OnUpdateState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;

        // Transition: Wait time is over -> Patrol
        if(Time.time > leaveTime) 
        {
            npcController.SwitchToState(npcController.PatrolState);
        }

        // Transition: Player spotted -> Flee
        if(npcController.CanSeePlayer || npcController.CanHearPlayer) 
        {
            //npcController.SwitchToState(npcController.FleeState);
            npcController.SwitchToState(npcController.AttackState);
        }
    }
}
