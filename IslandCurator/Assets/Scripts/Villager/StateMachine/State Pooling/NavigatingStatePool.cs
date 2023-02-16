using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigatingStatePool : StatePool<NavigatingState>
{
    protected override void CreateNewPoolState()
    {
        NavigatingState newState = new NavigatingState();
        newState.Initialize();
        _statePool.Enqueue(newState);
    }
}
