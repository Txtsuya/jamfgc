using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputBuffer inputBuffer = new InputBuffer();

    private InputAction move;
    private InputAction jump;
    private InputAction light;
    private InputAction medium;
    private InputAction heavy;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        // IMPORTANT: this is the per-player action instance
        var actions = playerInput.actions;

        // These names must match your Input Actions asset
        move   = actions["Move"];
        jump   = actions["Jump"];
        light  = actions["LightPunch"];
        medium = actions["MediumPunch"];
        heavy  = actions["HeavyPunch"];
    }

    private void OnEnable()
    {
        // Ensure only the gameplay map is enabled (adjust "Player" if your map name differs)
        playerInput.actions.Disable();
        playerInput.actions.FindActionMap("Player", true).Enable();

        move.performed += OnMovePerformed;
        move.canceled  += OnMoveCanceled;

        jump.performed += _ => inputBuffer.SetJump();
        light.performed += _ => inputBuffer.SetLightPunch();
        medium.performed += _ => inputBuffer.SetMediumPunch();
        heavy.performed += _ => inputBuffer.SetHeavyPunch();
    }

    private void OnDisable()
    {
        move.performed -= OnMovePerformed;
        move.canceled  -= OnMoveCanceled;

        jump.performed -= _ => inputBuffer.SetJump();     
        light.performed -= _ => inputBuffer.SetLightPunch();
        medium.performed -= _ => inputBuffer.SetMediumPunch();
        heavy.performed -= _ => inputBuffer.SetHeavyPunch();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>();

        // deadzone
        int x = (Mathf.Abs(v.x) > 0.5f) ? Mathf.RoundToInt(v.x) : 0;
        int y = (Mathf.Abs(v.y) > 0.5f) ? Mathf.RoundToInt(v.y) : 0;

        inputBuffer.SetMoveX(x);
        inputBuffer.SetMoveY(y);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        inputBuffer.SetMoveX(0);
        inputBuffer.SetMoveY(0);
    }

    public FrameInput ConsumeInput() => inputBuffer.Consume();
}