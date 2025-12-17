using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction powerUpAction;
    private InputAction shootAction;
    private InputAction elementSwitchLeftAction;
    private InputAction elementSwitchRightAction;

    public static Vector2 movement;
    public static bool powerUpPressed;
    public static bool shootPressed;
    public static bool shootHeld;
    public static bool elementSwitchLeftPressed;
    public static bool elementSwitchRightPressed;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        powerUpAction = playerInput.actions["Power Up"];
        shootAction = playerInput.actions["Shoot"];
        elementSwitchLeftAction = playerInput.actions["Element Left"];
        elementSwitchRightAction = playerInput.actions["Element Right"];
    }

    private void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
        powerUpPressed = powerUpAction.WasPressedThisFrame();
        shootPressed = shootAction.WasPressedThisFrame();
        shootHeld = shootAction.IsPressed();
        elementSwitchLeftPressed = elementSwitchLeftAction.WasPressedThisFrame();
        elementSwitchRightPressed = elementSwitchRightAction.WasPressedThisFrame();
    }
}
