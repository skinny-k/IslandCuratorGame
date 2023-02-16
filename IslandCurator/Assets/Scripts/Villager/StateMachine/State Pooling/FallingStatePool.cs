using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStatePool : StatePool<FallingState>
{
    protected override void CreateNewPoolState()
    {
        FallingState newState = new FallingState();
        newState.Initialize();
        _statePool.Enqueue(newState);
    }
}
