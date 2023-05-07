using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAction : MonoBehaviour, IColor
{
    private bool iJumped = false;
    private float coyoteCurrentTimer;
    public void Action(GameObject player, bool isPressed)
    {
        PlayerMovement moveScript = player.GetComponent<PlayerMovement>();
        PlayerChecks checkScript = player.GetComponent<PlayerChecks>();
        PlayerControl inputScript = moveScript.Input;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        
        if (checkScript.IsGrounded)
        {
            iJumped = false;
            coyoteCurrentTimer = moveScript.Data.CoyoteTime;
        }
        else
        {//aba
            coyoteCurrentTimer -= Time.deltaTime;
        }

        if (isPressed && !iJumped)
        {
            if (coyoteCurrentTimer > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveScript.Data.JumpForce);
                iJumped = true;
                return;
            }
        }
        else if (!isPressed && iJumped && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            iJumped = false;
        }
    }

    public void ResetAction(GameObject player)
    {
        
    }

}
