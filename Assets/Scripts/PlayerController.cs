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

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(joystick.Horizontal * moveSpeed, joystick.Vertical * moveSpeed);
        FlipSprite();
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}
