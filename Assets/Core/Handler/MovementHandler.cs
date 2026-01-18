using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 14f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    public bool isGrounded { get; private set; }

    private Vector2 standingSize;
    private Vector2 standingOffset;
    public Vector2 crouchingSize = new Vector2(0.5f, 0.8f);
    public Vector2 crouchingOffset = new Vector2(0f, 3.0f);


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        standingSize = col.size;
        standingOffset = col.offset;
    }

    public void Tick(FrameInput input, int pid, PlayerComponent self)
    {

        self.TickKnockback(rb);
        if (self.ProcessHitlag()) {
            return;
        }
        bool isBackWalking = false;
        bool isMoving = input.moveX > 0.5f || input.moveX < -0.5f;

        if (input.moveX > 0.5f && self.facing == PlayerComponent.Direction.Left) {
            self.isBlocking = true;
            isBackWalking = true;
        } else if (input.moveX < -0.5f && self.facing == PlayerComponent.Direction.Right) {
            self.isBlocking = true;
            isBackWalking = true;
        } else {
            self.isBlocking = false;
        }

        if (self.processMoveFrames(rb)) {
            return;
        }
        if (!(input.moveY < -0.5f)) {
            rb.linearVelocity = new Vector2(input.moveX * moveSpeed, rb.linearVelocity.y);
            self.CoordX = rb.transform.position.x;
        }
        animator.SetBool("isWalking", isMoving && !isBackWalking);
        animator.SetBool("isBackWalking", isBackWalking);

        if (input.jump && isGrounded) {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
        animator.SetBool("isGrounded", isGrounded);
        if (input.moveY < -0.5f) {
            self.isCrouching = true;
            col.size = crouchingSize;
            float standingBottom = standingOffset.y - standingSize.y / 2f;
            float newOffsetY = (standingBottom + crouchingSize.y) - 0.165f;
           // Debug.Log(newOffsetY);
            col.offset = new Vector2(standingOffset.x, newOffsetY);
        } else {
            self.isCrouching = false;
            col.size = standingSize;
            col.offset = standingOffset;
        }
        
        animator.SetBool("isCrouching", self.isCrouching);
        if (self.facing == PlayerComponent.Direction.Left) {
            spriteRenderer.flipX = true;
        } else if (self.facing == PlayerComponent.Direction.Right) {
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground")) {
            isGrounded = false;
        }
    }
}
