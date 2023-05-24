using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerCore player;
    private Animator animator;
    void Awake()
    {
        player = GetComponent<PlayerCore>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        animator.SetInteger("walk_Direction", (int)player.Controller.Axis.x);
        animator.SetBool("tongue", player.Controller.TongueButton);

        setJumpAndFall();

        if (player.ColorManager.CurrentColor.ColorData.Type == ColorType.Orange)
            animator.SetBool("isRunning", player.Controller.ColorButtonHold);
        else
            animator.SetBool("isRunning", false);     

    }

    private void setJumpAndFall()
    {
        if (player.ColorManager.CurrentColor.ColorData.Type == ColorType.Blue)
        {
            animator.SetBool("isJumping", player.Abilities.IsJumping);
        }
        if(player.Check.IsFalling)
        {
            animator.SetBool("isJumping", false);
        }
        if(!player.Check.OnGround() && !player.Check.IsFalling)
        {
            animator.SetBool("isJumping", true);
        }

        animator.SetBool("isFalling", player.Check.IsFalling);
    }


}
