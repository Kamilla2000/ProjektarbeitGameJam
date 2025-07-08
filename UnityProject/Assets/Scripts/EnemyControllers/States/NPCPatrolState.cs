using System;
using UnityEngine;

[Serializable]
public class NPCPatrolState : BaseState
{
    public Transform[] Waypoints;
    private int currentWaypointIndex = 0;

    /*public override void OnEnterState(BaseStateMachine controller)
    {
        NPCStateMachine npc = controller as NPCStateMachine;

        npc.SetSpeedMultiplier(0.5f);

        if (Waypoints == null || Waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints fehlen!");
            return;
        }

        // Wenn kein Ziel gesetzt ist, starte bei Index 0
        if (npc.NextPatrolPoint == Vector3.zero)
        {
            npc.NextPatrolPoint = Waypoints[0].position;
        }

        npc.SetDestionation(npc.NextPatrolPoint);
    }

    public override void OnUpdateState(BaseStateMachine controller)
    {
        NPCStateMachine npc = controller as NPCStateMachine;

        if (npc.CanSeePlayer || npc.CanHearPlayer)
        {
            npc.SwitchToState(npc.AttackState);
            return;
        }

        float distance = Vector3.Distance(controller.transform.position, npc.NextPatrolPoint);
        if (distance < 0.3f)
        {
            // Ziel für nächsten Durchlauf setzen
            int nextIndex = GetWaypointIndex(npc.NextPatrolPoint);
            nextIndex = (nextIndex + 1) % Waypoints.Length;
            npc.NextPatrolPoint = Waypoints[nextIndex].position;

            // Wechsel zu Idle
            npc.SwitchToState(npc.IdleState);
        }
    }

    public override void OnExitState(BaseStateMachine controller)
    {
        NPCStateMachine npc = controller as NPCStateMachine;
        npc.SetSpeedMultiplier(1f);
    }

    private int GetWaypointIndex(Vector3 point)
    {
        for (int i = 0; i < Waypoints.Length; i++)
        {
            if (Waypoints[i] != null && Vector3.Distance(Waypoints[i].position, point) < 0.2f)
            {
                return i;
            }
        }
        return 0;
    }*/
}