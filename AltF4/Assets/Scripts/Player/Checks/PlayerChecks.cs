using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    [SerializeField] private LayerMask solid;
    
    [Header("Wall Detector Variables")]
    [SerializeField] private Transform wallDetectorPoint;
    [SerializeField] private Vector2 wallDetectorSize;

    [Header("Ground Detector Variables")]
    [SerializeField] private Transform groundDetectorPoint;
    [SerializeField] private Vector2 groundDetectorSize;

    [Header("Slope Variables (LoL)")]
    [SerializeField] private float maxAngleSlope;
    [SerializeField] private float slopeDetectorDistance;
    [SerializeField] private float slopeDetectorOffset;
    [SerializeField] private float wallCheckDistance;
    
    private PlayerCore player;
    private CapsuleCollider2D capsule;
    public bool IsGrounded { get; private set; }
    public float LastTimeGrounded { get; private set;}
    public bool IsFalling { get; private set; }
    public bool isOnSlop { get; private set; }
    public bool IsFacingWall {get; private set;}
    public float SlopeAngle { get; private set; }
    public Vector2 SlopeDirection { get; private set; }

    void Awake()
    {
        player = GetComponent<PlayerCore>();
        capsule = GetComponent<CapsuleCollider2D>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded = OnGround();

        CoyoteTime();
        IsFacingWall = OnWall();

        IsFalling = OnFall();
    }
    /*
    private void SlopeDetector()
    {
        int direction = player.Movement.IsFacingRight ? 1 : -1;
        Vector2 pointRight = new Vector2(capsule.bounds.center.x + (capsule.bounds.extents.x - slopeDetectorOffset) * direction , capsule.bounds.center.y);
        Vector2 pointLeft = new Vector2(capsule.bounds.center.x - (capsule.bounds.extents.x - slopeDetectorOffset) * direction , capsule.bounds.center.y);

        RaycastHit2D hitRight = Physics2D.Raycast(pointRight, Vector2.down, slopeDetectorDistance, ground);
        RaycastHit2D hitLeft = Physics2D.Raycast(pointLeft, Vector2.down, slopeDetectorDistance, ground);

        bool hit = hitLeft || hitRight;

        var hitColor = hit ? Color.green : Color.red;

        Debug.DrawRay(pointRight, Vector2.down * slopeDetectorDistance, hitColor);
        Debug.DrawRay(pointLeft, Vector2.down * slopeDetectorDistance, hitColor);

        if (hitRight)
        {
            SlopeDirection = Vector2.Perpendicular(hitRight.normal).normalized;
            SlopeAngle = Vector2.Angle(hitRight.normal, Vector2.up);
            isOnSlop = SlopeAngle != 0;

            Debug.DrawRay(hitRight.point, SlopeDirection, Color.blue);
            Debug.DrawRay(hitRight.point, hitRight.normal, Color.magenta);

        }
        
        if (hitLeft)
        {
            SlopeDirection = Vector2.Perpendicular(hitLeft.normal).normalized;
            SlopeAngle = Vector2.Angle(hitLeft.normal, Vector2.up);
            isOnSlop = SlopeAngle != 0;

            Debug.DrawRay(hitLeft.point, SlopeDirection, Color.blue);
            Debug.DrawRay(hitLeft.point, hitLeft.normal, Color.magenta);
        }

    }
    */
    public bool OnGround()
    {
        var groundCheck = Physics2D.OverlapBox(groundDetectorPoint.position, groundDetectorSize, 0, solid);
        return groundCheck;
    }
    private bool OnWall()
    {
        var wallCheck = Physics2D.OverlapBox(wallDetectorPoint.position, wallDetectorSize, 0, solid);
        return wallCheck;
    }

    private void CoyoteTime()
    {
        if (OnGround())
        {
            LastTimeGrounded = player.Data.CoyoteTime;
        }
        else
        {
            LastTimeGrounded -= Time.deltaTime;
            if (LastTimeGrounded < 0)
            {
                LastTimeGrounded = 0;
            }
        }

    }

    public bool OnFall()
    {
        if (player.rb.velocity.y < 0 && !OnGround())
            return true;
        else
            return false;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (groundDetectorPoint == null) return;
        Gizmos.color = OnGround() ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundDetectorPoint.position, new Vector3(groundDetectorSize.x, groundDetectorSize.y, 0));

        if (wallDetectorPoint == null) return;
        Gizmos.color = OnWall() ? Color.green : Color.red;
        Gizmos.DrawWireCube(wallDetectorPoint.position, new Vector3(wallDetectorSize.x, wallDetectorSize.y, 0));
    }
    #endif
}
