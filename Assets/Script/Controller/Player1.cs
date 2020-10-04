using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Controller
{
    private bool wallJumping;
    private float touchingLeftOrRight;
    protected override void Move()
    {
        base.Move();
        
        if (isTouchingLeft)
        {
            touchingLeftOrRight = 1;
        }
        if (isTouchingRight)
        {
            touchingLeftOrRight = -1;
        }
        
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z)) && (isTouchingLeft || isTouchingRight) && !isGrounded)
        {
            wallJumping = true;
            Invoke("SetJumpingToFalse",0.1f);
        }
        if(wallJumping) rb.velocity = new Vector2(speed * 1.5f * touchingLeftOrRight, jumpForce*0.75f);
    }
    

    private void SetJumpingToFalse()
    {
        wallJumping = false;
    }
}
