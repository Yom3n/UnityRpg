using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTile : MonoBehaviour
{
    public event Action<Collider2D> OnTileTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered tile");
        Debug.Log(other.name);
        OnTileTriggered?.Invoke(other);
    }
}