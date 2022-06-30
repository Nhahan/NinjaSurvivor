using System;
using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FixedJoystick joystick;
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
        rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) *
                      (_movementSpeed + _movementSpeed * speedMult);

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
}
