using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class GrabbableItem : MonoBehaviour
{
    public abstract void Grab(PlayerPickup player);
    public abstract void Drop();

    protected bool _held = false;

    public bool Held
    {
        get => _held;
    }

    protected void OnMouseDown()
    {
        if (!_held)
        {
            Grab(PlayerPickup.Instance);
        }
    }
}
