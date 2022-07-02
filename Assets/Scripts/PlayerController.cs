using System;
using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private Animator animator;
    private Player _player;
    private float _movementSpeed;

    private void Start()
    {
        _player = GameManager.Instance.GetPlayer();
        _movementSpeed = _player.MovementSpeed.CalculateFinalValue();
    }

    private void FixedUpdate()
    {
        var speedMult = _player.FootworkTraining.CalculateFinalValue() / 20;
        rb.velocity = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical) *
                      (_movementSpeed + _movementSpeed * speedMult);
        // Debug.Log(joystick.Horizontal + " / " + joystick.Vertical);
        // Vector3 direction = Vector3.forward * variableJoystick.Vertical + 
        //                     Vector3.right * (variableJoystick.Horizontal * (_movementSpeed + _movementSpeed * speedMult));
        // rb.AddForce(direction  * Time.fixedDeltaTime, (ForceMode2D)ForceMode.VelocityChange);

        var hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        Run(hasHorizontalSpeed);
        FlipSprite(hasHorizontalSpeed);
    }

    private void Run(bool hasHorizontalSpeed)
    {
        animator.SetBool("isRunning", hasHorizontalSpeed);
    }

    private void FlipSprite(bool hasHorizontalSpeed)
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    public void ResetJoystick()
    {
        variableJoystick.SetInputReset();
    }
}
