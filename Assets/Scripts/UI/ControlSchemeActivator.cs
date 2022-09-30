using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSchemeActivator : MonoBehaviour
{
    [SerializeField] private GameObject keyboardMouseScheme;
    [SerializeField] private GameObject gamepadScheme;

    private static readonly string KeyboardMouseControlScheme = "Keyboard&Mouse";
    private static readonly string GamepadControlScheme = "Gamepad";

    public void OnControlsChanged(PlayerInput playerInput)
    {
        keyboardMouseScheme.SetActive(playerInput.currentControlScheme == KeyboardMouseControlScheme);
        gamepadScheme.SetActive(playerInput.currentControlScheme == GamepadControlScheme);
    }
}
