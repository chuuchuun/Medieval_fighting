using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour
{
    public Vector2 move;
    public Vector2 look;
    public bool attack;
    public bool jump;
    public bool crouch;

    public UnityEvent attackEvent;
    public UnityEvent jumpEvent;
    public UnityEvent crouchEvent;

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }

    public void OnAttack(InputValue value)
    {
        attack = value.isPressed;
        attackEvent?.Invoke();
    }
    public void OnAttackReleased()
    {
        attack = false;
        attackEvent?.Invoke();
    }
    public void OnJump(InputValue value)
    {
        jump = value.isPressed;
        jumpEvent?.Invoke();
    }

    public void OnCrouch(InputValue value)
    {
        crouch = value.isPressed;
        crouchEvent?.Invoke();
    }

    public void OnRun()
    {

    }
}
