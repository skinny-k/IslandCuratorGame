using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Navigator))]
public class GrabVillager : GrabbableItem
{
    [Header("Grab Feedback Settings")]
    [SerializeField] float _clickScale = 0.8f;
    [SerializeField] float _heldScale = 1.3f;
    [SerializeField] float _interpolationSpeed = 0.5f;
    
    PlayerPickup _playerHolding = null;
    Navigator _movement = null;
    float _scaleModifier = 1f;
    bool _held = false;
    
    void Start()
    {
        _movement = GetComponent<Navigator>();
    }
    
    protected override void Grab(PlayerPickup player)
    {
        _held = true;
        _scaleModifier = _clickScale;
        
        player.SetHeld(this);
        _playerHolding = player;
    }

    protected override void Drop()
    {
        _held = false;
        _scaleModifier = 1f;

        _playerHolding.ClearHeld();
        _movement.ReturnToBaseNode();
    }

    void OnMouseDown()
    {
        if (!_held)
        {
            Grab(PlayerPickup.Instance);
        }
    }

    void OnMouseUp()
    {
        if (_held)
        {
            Drop();
        }
    }

    void Update()
    {
        UpdateScale();
    }

    void UpdateScale()
    {
        if (transform.localScale.x > _scaleModifier)
        {
            float scaleThisFrame = Mathf.Clamp(transform.localScale.x - _interpolationSpeed * Time.deltaTime, _scaleModifier, _heldScale);
            transform.localScale = new Vector3(scaleThisFrame, scaleThisFrame, transform.localScale.z);
            if ((transform.localScale.x == _scaleModifier) &&
                (transform.localScale.y == _scaleModifier) &&
                (_scaleModifier == _clickScale))
            {
                _scaleModifier = _heldScale;
            }
        }
        else if (transform.localScale.x < _scaleModifier)
        {
            float scaleThisFrame = Mathf.Clamp(transform.localScale.x + 2 * _interpolationSpeed * Time.deltaTime, _clickScale, _scaleModifier);
            transform.localScale = new Vector3(scaleThisFrame, scaleThisFrame, transform.localScale.z);
        }
    }
}
