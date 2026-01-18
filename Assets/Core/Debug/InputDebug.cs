using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputDebug : MonoBehaviour
{
    private PlayerInput pi;
    private InputAction move;

    void Awake()
    {
        Debug.Log("InputDebug Awake");
        pi = GetComponent<PlayerInput>();
        move = pi.actions["Move"]; // must match your action name
    }

    void OnEnable()
    {
        Debug.Log("InputDebug OnEnable");
        pi.actions.Enable();
        pi.actions.FindActionMap("Player", true).Enable();
    }

    void Update()
    {
        Vector2 v = move.ReadValue<Vector2>();
        if (v.sqrMagnitude > 0.01f)
            Debug.Log($"{name} move={v} devices={string.Join(", ", pi.devices)} map={pi.currentActionMap?.name}");
    }
}
