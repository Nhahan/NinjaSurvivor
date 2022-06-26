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
    [SerializeField] private Player player;
    
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) * player.MovementSpeed.CalculateFinalValue();

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
