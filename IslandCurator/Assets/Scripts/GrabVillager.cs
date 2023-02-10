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

    [Header("Drop Settings")]
    [SerializeField] float _dropDistance = 3f;
    [SerializeField] float _fallSpeed = 10f;

    [Header("Shadow Settings")]
    [SerializeField] SpriteRenderer _shadow = null;
    [SerializeField] float _baseShadowDistance = -0.25f;
    [SerializeField] float _baseAlpha = 0.05f;
    [SerializeField] float _heldAlpha = 0.2f;
    [SerializeField] float _positionInterpolationSpeed = 10f;
    [SerializeField] float _alphaInterpolationSpeed = 1.5f;
    
    PlayerPickup _playerHolding = null;
    Navigator _movement = null;
    Vector3 _fallTarget;
    float _scaleModifier = 1f;
    bool _held = false;
    bool _falling = false;
    
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
        _falling = true;
        _fallTarget = transform.position - new Vector3(0, _dropDistance, 0);
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
                float scaleThisFrame = Mathf.Clamp(1 + Mathf.Abs(transform.position.y - _fallTarget.y), 1, _heldScale);
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
        if (_falling && _shadow.color.a > _baseAlpha)
        {
            float alphaThisFrame = Mathf.Clamp(_shadow.color.a - _alphaInterpolationSpeed * Time.deltaTime, _baseAlpha, _heldAlpha);
            _shadow.color = new Color(_shadow.color.r, _shadow.color.g, _shadow.color.b, alphaThisFrame);
        }
        else if (_held && _shadow.color.a < _heldAlpha)
        {
            float alphaThisFrame = Mathf.Clamp(_shadow.color.a + _alphaInterpolationSpeed * Time.deltaTime, _baseAlpha, _heldAlpha);
            _shadow.color = new Color(_shadow.color.r, _shadow.color.g, _shadow.color.b, alphaThisFrame);
        }

        if (_held && _shadow.transform.localPosition.y != _baseShadowDistance - _dropDistance)
        {
            float yThisFrame = Mathf.Clamp(_shadow.transform.localPosition.y - _positionInterpolationSpeed * Time.deltaTime, _baseShadowDistance - _dropDistance, _baseShadowDistance);
            _shadow.transform.localPosition = new Vector3(0, yThisFrame, _shadow.transform.localPosition.z);
        }
        else if (_falling && _shadow.transform.localPosition.y != _baseShadowDistance)
        {
            float yThisFrame = Mathf.Clamp(_shadow.transform.localPosition.y + _fallSpeed * Time.deltaTime, _baseShadowDistance - _dropDistance, _baseShadowDistance);
            _shadow.transform.localPosition = new Vector3(0, yThisFrame, _shadow.transform.localPosition.z);
        }
    }

    void Fall()
    {
        transform.position = Vector3.MoveTowards(transform.position, _fallTarget, _fallSpeed * Time.deltaTime);
        
        if (transform.position == _fallTarget)
        {
            _falling = false;
            _movement.ReturnToBaseNode();
        }
    }
}
