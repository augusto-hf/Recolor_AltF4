using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAction : MonoBehaviour, IColor
{
    [SerializeField] private PlayerControl inputScript;
    public void Action(GameObject player, bool isPressed)
    {
        PlayerMoviment moveScript = player.GetComponent<PlayerMoviment>();
        PlayerChecks checkScript = player.GetComponent<PlayerChecks>();

        if (checkScript.IsGrounded && isPressed)
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, moveScript.Data.JumpForce);
    }
}
