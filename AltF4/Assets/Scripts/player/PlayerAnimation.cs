using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerCore player;
    private Animator animator;
    [SerializeField] private ParticleSystem dust;
    [SerializeField] private Transform pointDust;
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

        if (player.Color.CurrentColor.Type == ColorType.Orange)
            animator.SetBool("isRunning", player.Controller.ColorButton);
        else
            animator.SetBool("isRunning", false);


            

    }

    private void setJumpAndFall()
    {
        if (player.Color.CurrentColor.Type == ColorType.Blue)
        {
            animator.SetBool("isJumping", player.Controller.ColorButton);
        }

        //else
           //animator.SetBool("isJumping", false);

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
