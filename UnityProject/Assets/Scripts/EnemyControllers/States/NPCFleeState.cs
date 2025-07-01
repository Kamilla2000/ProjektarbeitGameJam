using System;
using UnityEngine;

[Serializable]
public class NPCFleeState : BaseState
{
    // Distance before switch back to idle
    public float DistanceToFlee;

    public override void OnUpdateState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;

        // ACTION: Calculate a position far away from the player
        Vector3 fleeDestination = npcController.transform.position +
            (npcController.transform.position - npcController.PlayerPosition).normalized * DistanceToFlee;
        npcController.SetDestionation(fleeDestination);

        // TRANSITION - LOGIC
        float sqrtDistanceToPlayer = (npcController.PlayerPosition - npcController.transform.position).sqrMagnitude;

        if (sqrtDistanceToPlayer > DistanceToFlee * DistanceToFlee) 
        {
            npcController.SwitchToState(npcController.IdleState);
        }
    }
}