using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GrabbableItem : MonoBehaviour
{
    protected abstract void Grab(PlayerPickup player);
    protected abstract void Drop();
}
