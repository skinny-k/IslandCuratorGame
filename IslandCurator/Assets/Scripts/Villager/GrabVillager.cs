using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabVillager : GrabbableItem
{
    [Header("Grab Feedback Settings")]
    [SerializeField] float _clickScale = 0.8f;
    [SerializeField] float _heldScale = 1.3f;
    [SerializeField] float _interpolationSpeed = 0.5f;

    [Header("Drop Settings")]
    [SerializeField] float _dropDistance = 3f;
    [SerializeField] float _fallSpeed = 10f;

    [Header("Shadow Settings")]
    [SerializeField] SpriteRenderer _shadow = null;
    [SerializeField] float _alphaChangeAmount = 0.15f;
    [SerializeField] float _alphaInterpolationSpeed = 1.5f;
    
    PlayerPickup _playerHolding = null;
    Navigator _movement = null;
    Vector3 _fallOffset;
    Vector3 _fallTarget;
    Vector3 _shadowOffset;
    Vector3 _shadowTarget;
    float _targetAlpha;
    float _scaleModifier = 1f;
    bool _falling = false;

    public event Action OnGrab;
    public event Action OnDrop;
    public event Action OnLand;
    
    public override void Grab(PlayerPickup player)
    {
        _held = true;
        _scaleModifier = _clickScale;
        
        player.SetHeld(this);
        _playerHolding = player;
        _fallOffset = new Vector3(0, -_dropDistance, 0);

        _shadowTarget = _fallOffset;
        _targetAlpha = _shadow.GetComponent<SpriteRenderer>().color.a + _alphaChangeAmount;

        OnGrab?.Invoke();
    }

    public override void Drop()
    {
        _held = false;
        _scaleModifier = 1f;

        _playerHolding.ClearHeld();
        _fallTarget = transform.localPosition + _fallOffset;
        _falling = true;

        _shadowTarget = _shadowOffset;
        _targetAlpha = _shadow.GetComponent<SpriteRenderer>().color.a - _alphaChangeAmount;

        OnDrop?.Invoke();
    }

    void Start()
    {
        _movement = GetComponent<Navigator>();
        _shadowOffset = _shadow.transform.localPosition;
    }

    void Update()
    {
        if (_falling || _held)
        {
            if (_falling)
            {
                Fall();
            }
            UpdateShadow();
        }
        UpdateScale();
    }

    void OnMouseUp()
    {
        if (_held)
        {
            Drop();
        }
    }

    void UpdateScale()
    {
        if (transform.localScale.x > _scaleModifier)
        {
            if (!_falling)
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
            else if (_falling)
            {
                float scaleThisFrame = Mathf.Clamp(1 + Mathf.Abs(transform.localPosition.y - _fallOffset.y), 1, _heldScale);
                transform.localScale = new Vector3(scaleThisFrame, scaleThisFrame, transform.localScale.z);
            }
        }
        else if (transform.localScale.x < _scaleModifier)
        {
            float scaleThisFrame = Mathf.Clamp(transform.localScale.x + 2 * _interpolationSpeed * Time.deltaTime, _clickScale, _scaleModifier);
            transform.localScale = new Vector3(scaleThisFrame, scaleThisFrame, transform.localScale.z);
        }
    }

    void UpdateShadow()
    {
        if (_shadow.color.a != _targetAlpha)
        {
            float alphaThisFrame = Mathf.Clamp(_shadow.color.a + Mathf.Sign(_targetAlpha - _shadow.color.a) * _alphaInterpolationSpeed * Time.deltaTime, 0, 1);
            _shadow.color = new Color(_shadow.color.r, _shadow.color.g, _shadow.color.b, alphaThisFrame);
        }

        if (_shadow.transform.localPosition != _shadowTarget)
        {
            _shadow.transform.localPosition = Vector3.MoveTowards(_shadow.transform.localPosition, _shadowTarget, _fallSpeed * Time.deltaTime);
        }
    }

    void Fall()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _fallTarget, _fallSpeed * Time.deltaTime);
        
        if (transform.localPosition == _fallTarget)
        {
            _falling = false;
            OnLand?.Invoke();
        }
    }
}
