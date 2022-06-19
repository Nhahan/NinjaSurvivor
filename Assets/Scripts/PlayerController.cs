using System.Collections;
using System.Collections.Generic;
using Status;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator animator;
    [SerializeField] private Player player;

    public static Vector2 LatestDirection { get; private set; }
    
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) * player.MovementSpeed.CalculateFinalValue();
        LatestDirection = new Vector2(
            rb.velocity.x / Mathf.Abs(rb.velocity.x),
            rb.velocity.y / Mathf.Abs(rb.velocity.y));

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
