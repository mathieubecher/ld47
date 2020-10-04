using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Controller
{
    public bool secondJump = true;
    protected override void Move()
    {
        base.Move();
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) && !isGrounded && secondJump)
        {
            secondJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (isGrounded) secondJump = true;
    }
}
