using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatePool<T> : MonoBehaviour where T : State
{
    protected abstract void CreateNewPoolState();
    // The body of this function in the derived class should resemble the following, where T is the specific type
    // {
    //     T newState = new T();
    //     newState.Initialize();
    //     _statePool.Enqueue(newState);
    // }
    
    [SerializeField] protected int _startingPoolSize = 10;

    protected Queue<T> _statePool = new Queue<T>();
    
    protected virtual void Start()
    {
        CreateInitialPool();
    }

    protected virtual void CreateInitialPool()
    {
        for (int i = 0; i < _startingPoolSize; i++)
        {
            CreateNewPoolState();
        }
    }
    
    public virtual T ActivateFromPool()
    {
        if (_statePool.Count == 0)
        {
            CreateNewPoolState();
        }
        
        T stateToActivate = _statePool.Dequeue();
        stateToActivate.Initialize();
        
        return stateToActivate;
    }

    public virtual void ReturnToPool(T stateToReturn)
    {
        stateToReturn.Initialize();
        _statePool.Enqueue(stateToReturn);
    }

    public bool HasActiveChildren()
    {
        foreach (T state in _statePool)
        {
            if (state.isEnabled)
            {
                return true;
            }
        }

        return false;
    }
}
