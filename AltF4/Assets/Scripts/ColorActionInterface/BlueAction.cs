using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAction : MonoBehaviour, IColor
{
    private bool iJumped = false;
    private float coyoteCurrentTimer;
    public void Action(GameObject player, bool isPressed)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        
    }

    public void ResetAction(GameObject player)
    {
        
    }

}
