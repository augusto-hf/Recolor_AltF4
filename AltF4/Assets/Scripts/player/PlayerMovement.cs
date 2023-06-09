using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D playerSlope;
    [SerializeField] private PhysicsMaterial2D playerDefault;
    [SerializeField] private PhysicsMaterial2D noFriction;
    
    public bool canMove = false;

    [SerializeField] private Vector2 speed;
    private PlayerCore player;
    private Rigidbody2D rb;
    private float currentMaxSpeed;
    private bool isJumping;
    private bool isFacingRight;

    public Vector2 Velocity { get => rb.velocity; }
    public bool HasBluePassive { get; private set;}
    public bool IsFacingRight { get => isFacingRight; }
    
    private void Awake()
    {
        player = GetComponent<PlayerCore>();
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = this.transform.rotation.eulerAngles.y == 0;


        rb.gravityScale = player.Data.GravityScale;
        currentMaxSpeed = player.Data.MaxHorizontalSpeed;
    }

    private void FixedUpdate()
    {
        if (player.Health.IsDead)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            return;            
        }
        else
        {
            rb.isKinematic = false;
        }

        Debug.DrawRay(this.transform.position, rb.velocity.normalized, Color.cyan);
        Checkers();
        HorizontalMovement();
        VerticalMovement();
        speed = Velocity;
    }

    private void Checkers()
    {

        if (player.Check.IsGrounded)
        {
            HasBluePassive = false;
            isJumping = false;

        }

        if (player.Check.isOnSlop && player.Controller.Axis.x == 0)
        {
            rb.sharedMaterial = playerSlope;
        }
        else if (player.Check.isOnSlop && player.Controller.Axis.x != 0)
        {
            rb.sharedMaterial = noFriction;
        }
        else
        {
            rb.sharedMaterial = playerDefault;
        }
        
    }

    #region Horizontal Movement

    private void HorizontalMovement()
    {
        Flip();
        Walk();
        
    }

    private void Walk()
    {
        if (!canMove) return;

        if (player.Check.IsFacingWall)
        {
            Debug.Log("stuck wall");
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (player.Check.isOnSlop && !isJumping)
        {
            
            float targetVelocityX = player.Check.SlopeDirection.x * -player.Controller.Axis.x * currentMaxSpeed;
            float targetVelocityY = player.Check.SlopeDirection.y * -player.Controller.Axis.x * currentMaxSpeed;
            Vector2 targetVelocity = new Vector2(targetVelocityX, targetVelocityY);

            Debug.Log(targetVelocity);
            
            Vector2 speedToReach = targetVelocity - rb.velocity;
            
            float accelRate = Mathf.Abs(targetVelocity.magnitude) > 0.01f ? player.Data.HorizontalAcceleration : player.Data.HorizontalDeceleration;

            rb.AddForce(speedToReach * accelRate);

        }
        else
        {
            float targetVeloticy = player.Controller.Axis.x * currentMaxSpeed;

            float speedToReach = targetVeloticy - rb.velocity.x;
            
            float accelRate = Mathf.Abs(targetVeloticy) > 0.01f ? player.Data.HorizontalAcceleration : player.Data.HorizontalDeceleration;
            
            float xMovement  = speedToReach * accelRate;
            
            rb.AddForce(Vector2.right * xMovement , ForceMode2D.Force);
            
        }

        //rb.velocity = new Vector2(player.Controller.Axis.x * player.Data.MaxHorizontalSpeed, rb.velocity.y);

    }
    public void SetMaxSpeed(float maxSpeed)
    {
        currentMaxSpeed = maxSpeed;
    }
    private void Flip()
    {
        if ((isFacingRight && player.Controller.Axis.x < 0 || !isFacingRight && player.Controller.Axis.x > 0))
        {
            if (!player.Controller.TongueButton)
            {
                isFacingRight = !isFacingRight;
                this.transform.Rotate(0, 180, 0);
            }
            else
                return;
        }
    }

    #endregion 
    
    #region Vertical Movement
    
    private void VerticalMovement()
    {
        Fall();
    }

    private void Fall()
    {
        if (player.Check.IsFalling)
        {
            rb.gravityScale = player.Data.GravityScale * player.Data.FallMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -player.Data.MaxFallSpeed));
        }
        else
        {
            rb.gravityScale = player.Data.GravityScale;
        }
    }

    #endregion

    #region Jump

    public void JumpForceApply()
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, player.Data.JumpForce);
    }

    public void JumpCutForceApply()
    {
        rb.AddForce(Vector2.down * rb.velocity.y * player.Data.JumpCutMultiplier, ForceMode2D.Impulse);
    }

    public void SetBluePassive() => HasBluePassive = true;
    
    #endregion
    

}
