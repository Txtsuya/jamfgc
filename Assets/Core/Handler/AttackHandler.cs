using UnityEditor.UI;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public LayerMask hurtboxLayer;
    public MoveData lightPunchMoveData;
    public MoveData mediumPunchMoveData;    
    public MoveData heavyPunchMoveData;
    private Rigidbody2D rb;
    public Animator animator;

    public bool isAttacking = false;

    public bool isActiveFrame = false;

    public int directionMultiplier = 1;
    private int moveFrameCount = 0;
    private int moveHitCount = 0;
    public MoveData currentMoveData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    public void Tick(FrameInput input,
                    int pid,
                    PlayerComponent self,
                    PlayerComponent other)
    {
        if (!isAttacking) {
            switch (input) {
                case { lightPunch: true }:
                    StartMove(lightPunchMoveData, self, "LightPunch");
                    break;
                case { mediumPunch: true }:
                    StartMove(mediumPunchMoveData, self, "MediumPunch");
                    break;
                case { heavyPunch: true }:
                    StartMove(heavyPunchMoveData, self, "HeavyPunch");
                    break;
                default:
                    break;
            }
        } else {
            ProcessMove(self, other);
        }

    }

    private void StartMove(MoveData moveData, PlayerComponent self, string animationTrigger)
    {
        isAttacking = true;
        moveFrameCount = 0;
        currentMoveData = moveData;
        animator.SetTrigger(animationTrigger);
        if (self.facing == PlayerComponent.Direction.Left) {
            directionMultiplier = -1;
        } else {

            directionMultiplier = 1;
        }
    }


private bool isBlocking(MoveData move, PlayerComponent self, PlayerComponent other) {
        if (move.moveType == MoveData.MoveType.Medium && other.isBlocking)
        {
            return true;
        }
        if (move.moveType == MoveData.MoveType.Low && other.isBlocking && other.isCrouching)
        {
            return true;
        }
        if (move.moveType == MoveData.MoveType.High && other.isBlocking && !other.isCrouching)
        {
            return true;
        }
        return false;
    }
    private void ProcessMove(PlayerComponent self, PlayerComponent other)
    {
        moveFrameCount++;
        isActiveFrame = false;
        if (moveFrameCount > currentMoveData.totalFrames){
            isAttacking = false;
            moveFrameCount = 0;
            moveHitCount = 0;
            return;
        }
        
        if (moveFrameCount < currentMoveData.startupFrames) {
            return;
        }

        if (moveFrameCount < currentMoveData.activeFrames + currentMoveData.startupFrames) {
            if (moveHitCount >= currentMoveData.maxHits) {
                return;
            }
            
            Vector2 hitboxPos = rb.position +
                new Vector2(
                    currentMoveData.hitboxOffset.x * directionMultiplier,
                    currentMoveData.hitboxOffset.y
                );
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                hitboxPos,
                currentMoveData.hitboxSize,
                0f,
                hurtboxLayer
            );
            
            foreach (Collider2D hit in hits)
            {
                Debug.Log("Hit: " + hit.name);
                if (isBlocking(currentMoveData, self, other)) {
                    other.ApplyKnockback(
                        currentMoveData.blockPushback,
                        currentMoveData.blockPushbackForce,
                        currentMoveData.blockPushbackFrames,
                        directionMultiplier
                    );
                    other.ApplyHitlag(currentMoveData.blockStunFrames);
                    Debug.Log("Blocked!");
                    moveHitCount++;
                    continue;
                }
                
                other.Damage(currentMoveData.damage);
                moveHitCount++;
                other.ApplyHitlag(currentMoveData.hitlagFrames);
                other.ApplyKnockback(
                    currentMoveData.knockbackDirection,
                    currentMoveData.knockbackForce,
                    currentMoveData.knockbackFrames,
                    directionMultiplier
                );
            }
            
            isActiveFrame = true;
        }
    }
}
