using UnityEngine; 


public class PlayerComponent
{

    public enum Direction { Left, Right }
    public PlayerData playerData;
    public int playerIndex;
    public int playerHealth;
    public int ressourceCount;

    public float CoordX = 0.0f;

    private int hitLagFrames = 0;

    private int IFrames = 0;

    private Vector2 kbDirection = Vector2.zero;
    private float kbForce = 0f;
    private int kbFrames = 0;
    private int kbDecay = 1;

    public Direction facing = Direction.Right;

    public bool isCrouching = false;
    
    public bool isBlocking = false;
    

    public PlayerComponent(PlayerData data, int index )
    {
        playerData = data;
        playerIndex = index;
        playerHealth = playerData.maxHealth;
        ressourceCount = 0;
    }


    public void Damage(int damage)
    {
        Debug.Log("player " + playerIndex + " took " + damage + " damage. They're now at " + (playerHealth - damage) + " health.");
        playerHealth -= damage;
        if (playerHealth < 0) playerHealth = 0;
    }

    public bool ProcessHitlag()
    {
        if (hitLagFrames > 0) {
            hitLagFrames--;
            return true;
        }
        return false;
    }
    public void ApplyHitlag(int frames)
    {
        hitLagFrames = frames;
    }

    public void ApplyKnockback(Vector2 direction, float force, int frames, int multiplier)
    {
        //kbDirection = direction.normalized * multiplier;
        kbDirection.x = direction.normalized.x * multiplier;
        kbDirection.y = direction.normalized.y;
        kbForce = force;
        kbFrames = frames;
    }

    public void TickKnockback(Rigidbody2D rb)
    {
        if (kbFrames > 0)
        {
            rb.AddForce(kbDirection * kbForce, ForceMode2D.Impulse);
            kbFrames--;
            kbForce = Mathf.Max(0, kbForce - kbDecay);
        }
    }


    // wrapper tbh
    public void Hit(MoveData move)
    {
        if (IFrames > 0) {
            // process I frames hit
        } else {
            Damage(move.damage);
        }
        //ApplyHitlag(move.hitlagFrames);
        //ApplyKnockback(move.knockbackDirection, move.knockbackForce, move.knockbackFrames);
    }

}