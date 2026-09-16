using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform playerTransform;

    private float _gravity = -9.81f;
    private float _verticalVelocity;
    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        InputController();
    }

    private void InputController()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            input.x = Keyboard.current.aKey.isPressed ? -1f : 0f;
            input.x += Keyboard.current.dKey.isPressed ? 1f : 0f;
            input.y = Keyboard.current.wKey.isPressed ? 1f : 0f;
            input.y += Keyboard.current.sKey.isPressed ? -1f : 0f;
        }

        if (Gamepad.current != null)
        {
            Vector2 joystickInput = Gamepad.current.leftStick.ReadValue();

            if (joystickInput.sqrMagnitude > 0.01f)
            {
                input = joystickInput;
            }
        }

        Vector3 move = playerTransform.right * input.x + playerTransform.forward * input.y;

        if (move.sqrMagnitude > 1f)
        {
            move.Normalize();
        }

        if (_controller.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;

        move.y = _verticalVelocity;

        Vector3 velocity = new Vector3(move.x * moveSpeed, move.y, move.z * moveSpeed);

        _controller.Move(velocity * Time.deltaTime);
    }

}
