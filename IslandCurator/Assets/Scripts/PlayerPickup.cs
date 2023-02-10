using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public static PlayerPickup Instance = null;

    GrabbableItem _heldItem = null;

    public GrabbableItem HeldItem
    {
        get => _heldItem;
    }
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (_heldItem != null)
        {
            Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            _heldItem.transform.position = new Vector3(mousePosInWorld.x, mousePosInWorld.y, _heldItem.transform.position.z);
        }
    }

    public void SetHeld(GrabbableItem item)
    {
        _heldItem = item;
    }

    public void ClearHeld()
    {
        SetHeld(null);
    }
}
