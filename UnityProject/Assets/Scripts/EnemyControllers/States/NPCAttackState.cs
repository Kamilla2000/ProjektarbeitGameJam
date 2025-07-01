using System;
using UnityEngine;

[Serializable]
public class NPCAttackState : BaseState
{
    public float AttackRange = 2f;          // Distanz zum Angriff
    public float StopDistance = 1.5f;       // Distanz, ab der NPC stehen bleibt

    private bool isAttacking = false;

    public override void OnEnterState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;
        npcController.SetSpeedMultiplier(1f); // Volle Geschwindigkeit
        isAttacking = false;
    }

    public override void OnUpdateState(BaseStateMachine controller)
    {
        NPCStateMachine npcController = controller as NPCStateMachine;

        Vector3 playerPosition = npcController.PlayerPosition;
        float distanceToPlayer = Vector3.Distance(npcController.transform.position, playerPosition);

        if (distanceToPlayer > AttackRange)
        {
            // Auf Spieler zu bewegen
            npcController.SetDestionation(playerPosition);
            npcController.SetSpeedMultiplier(1f);
            isAttacking = false;
        }
        else
        {
            // Stoppen und Angriff starten
            npcController.SetDestionation(npcController.transform.position);

            if (!isAttacking)
            {
                isAttacking = true;
                npcController.AttackPlayer();
            }
        }

        // Wenn Spieler zu weit weg, zurück zu Idle (oder andere State)
        float maxAttackDistance = AttackRange * 2f;
        if (distanceToPlayer > maxAttackDistance)
        {
            npcController.SwitchToState(npcController.IdleState);
        }
    }
}