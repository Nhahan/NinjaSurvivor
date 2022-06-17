using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] FixedJoystick joystick;
    [SerializeField] Animator animator;
    [SerializeField] float moveSpeed;

    void Start()
    {

    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(joystick.Horizontal * moveSpeed, joystick.Vertical * moveSpeed);
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
