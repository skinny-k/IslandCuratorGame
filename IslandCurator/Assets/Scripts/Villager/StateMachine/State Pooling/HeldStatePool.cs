using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldStatePool : StatePool<HeldState>
{
    protected override void CreateNewPoolState()
    {
        HeldState newState = new HeldState();
        newState.Initialize();
        _statePool.Enqueue(newState);
    }
}
