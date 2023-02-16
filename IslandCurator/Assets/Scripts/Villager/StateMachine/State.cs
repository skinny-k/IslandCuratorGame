using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : IInitializable
{
    public bool isEnabled
    {
        get => _currentStateMachine == null;
    }
    
    protected StateMachineMB _currentStateMachine = null;
    protected float _stateDuration = 0;

    public void Initialize()
    {
        _stateDuration = 0;
    }
    
    public virtual void Enter() 
    {
        Initialize();
    }

    public virtual void Exit() 
    {
        _stateDuration = 0;
        _currentStateMachine = null;
    }

    public virtual void Update() 
    {
        _stateDuration += Time.deltaTime;
    }

    public virtual void FixedUpdate() 
    { 
        //
    }
}
