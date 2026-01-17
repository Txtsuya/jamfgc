using UnityEngine;

public class DebugHitboxes: MonoBehaviour
{
    private AttackHandler attackHandler;

    private void Awake()
    {
        attackHandler = GetComponent<AttackHandler>();
    }

    private void OnDrawGizmos()
    {
        if (attackHandler == null) return;

        // Draw active hitboxes
        if (attackHandler.isAttacking)
        {
            MoveData currentMove = attackHandler.currentMoveData;
            if (currentMove != null)
            {
                Gizmos.color = Color.red;
                Vector2 hitboxPosition = (Vector2)transform.position + 
                    new Vector2(
                    currentMove.hitboxOffset.x * -attackHandler.directionMultiplier,
                    currentMove.hitboxOffset.y
                );
                Gizmos.DrawWireCube(hitboxPosition, currentMove.hitboxSize);
            }
        }
    }
}