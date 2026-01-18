using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject
{

    public enum MoveType
    {
        Medium,
        Low,
        High,
        Overhead,
        Grab,
        Unblockable
    }
    public string moveId = "move_001"; // the id of the move
    public Vector2 hitboxOffset = Vector2.zero; // where the move hitbox spawns relative to the player
    public Vector2 hitboxSize = Vector2.zero; // width & height of the hitbox
    public int damage = 10; // damage dealt by the move
    public int startupFrames = 5; // frames before the move becomes active
    public int activeFrames = 3; // frames during which the move can hit
    public int recoveryFrames = 10; // how long after the active frames before the player can act again

    public int maxHits = 1; // maximum number of hits the move can deal

    public int hitlagFrames = 5; // how many frames the opponent is stunned on hit

    public int knockbackForce = 5; // how strong the move send the opponent flying
    public int knockbackFrames = 10; // how long the knockback lasts

    public Vector2 blockPushback = new Vector2(1f, 0.5f); // how far the opponent is pushed back when blocking
    public int blockPushbackFrames = 5; // how long the block pushback lasts
    public int blockPushbackForce = 1; // how strong the block pushback is
    public int blockStunFrames = 10; // how long the opponent is stunned when blocking
    // if blockstun is greater than the move's recovery frames  + the next move startup frames, it means the player
    // can act before the opponent recovers from blockstun
    // so to ensure we have no infinite blockstun loops, we need to ensure
    // that no character have a move with blockstun greater than their fastest move's AND no pushback
    // if we have pushback it's fine since the opponent is gonna get moved away
    public Vector2 knockbackDirection = new Vector2(1f, 1f); // direction of the knockback
    public int kbDecay = 1; // how quickly the knockback force decays over time
    public MoveType moveType = MoveType.Medium; // type of the move
    public int totalFrames
    {
        get { return startupFrames + activeFrames + recoveryFrames; }
    }

    

    
}
