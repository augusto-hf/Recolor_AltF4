using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerColorAction : MonoBehaviour
{
    private PlayerCore player;
    [SerializeField] private GameObject StartingColorReference;
    private IColor currentColor;
    private BlobManager lastBlob;

    public IColor CurrentColor { get => currentColor; }


    private void Awake()
    {
        GiveNoColor();
    }

    private void Start()
    {
        player = GetComponent<PlayerCore>();
    }

    void Update()
    {
        currentColor?.Action(player);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        TakeColor(other.gameObject);
    }
    
    public void TakeColor(GameObject colorObject)
    {
        if (colorObject.CompareTag("ColorPower"))
        {
            if (lastBlob != null)
                lastBlob.RespawnPower();

            lastBlob = colorObject.gameObject.GetComponentInParent<BlobManager>();
            lastBlob.PickPower();
            currentColor.ResetAction(player);
            currentColor = lastBlob.blobColor;

            player.PickColor(lastBlob.nameColor);
        }
    }

    public void GiveNoColor()
    {
        currentColor = StartingColorReference.gameObject.GetComponent<IColor>();
    }
}
