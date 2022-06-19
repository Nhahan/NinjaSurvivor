using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] Animator animator;
    [SerializeField] Player player;

    public static Vector2 LatestDirection { get; private set; }

    void Start()
    {

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(joystick.Horizontal, joystick.Vertical) * player.MovementSpeed.CalculateFinalValue();
        LatestDirection = new Vector2(
            rb.velocity.x / Mathf.Abs(rb.velocity.x),
            rb.velocity.y / Mathf.Abs(rb.velocity.y));

        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        Run(hasHorizontalSpeed);
        FlipSprite(hasHorizontalSpeed);
    }

    void Run(bool hasHorizontalSpeed)
    {
        animator.SetBool("isRunning", hasHorizontalSpeed);
    }

    void FlipSprite(bool hasHorizontalSpeed)
    {
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}
