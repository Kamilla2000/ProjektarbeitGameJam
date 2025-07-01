using System;
using UnityEngine;

[Serializable]
public class NPCPatrolState : BaseState
{
    public Transform[] Waypoints;

    private int currentWaypointIndex;

    private Vector3 targetPosition;

    public override void OnEnterState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;

        npcController.SetSpeedMultiplier(0.5f);

        if (targetPosition == Vector3.zero)
        {
            targetPosition = Waypoints[0].position;
        }

        npcController.SetDestionation(targetPosition);
    }

    public override void OnUpdateState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;

        // Transition: Waypoint reached -> Idle
        float sqrtDistance = (controller.transform.position - targetPosition).sqrMagnitude;
        if (sqrtDistance < 0.1f)
        {
            targetPosition = GetNextWaypoint();
            npcController.SwitchToState(npcController.IdleState);
        }

        // Transition: Player spotted -> Flee
        if (npcController.CanSeePlayer || npcController.CanHearPlayer)
        {
            //npcController.SwitchToState(npcController.FleeState);
            npcController.SwitchToState(npcController.AttackState);
        }
    }

    public override void OnExitState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;
        npcController.SetSpeedMultiplier(1);
    }

    private Vector3 GetNextWaypoint()
    {
        currentWaypointIndex = ++currentWaypointIndex % Waypoints.Length;
        return Waypoints[currentWaypointIndex].position;
    }
}
