using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingStatePool : StatePool<WanderingState>
{
    protected override void CreateNewPoolState()
    {
        WanderingState newState = new WanderingState();
        newState.Initialize();
        _statePool.Enqueue(newState);
    }
}
