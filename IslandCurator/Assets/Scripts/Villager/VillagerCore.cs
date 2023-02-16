using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerCore : MonoBehaviour, IInitializable
{
    public GrabbableItem GrabbableBody { get; private set; }
    public Navigator Movement { get; private set; }
    public VillagerAssignment Assignment { get; private set; }
    public VillagerSwing GrabSwing { get; private set; }
    
    public void Initialize()
    {
        //
    }

    void Reset()
    {
        GrabbableBody = AddComponentOfType<GrabVillager>(gameObject);
        Movement = AddComponentOfType<Navigator>(gameObject);
        Assignment = AddComponentOfType<VillagerAssignment>(gameObject);
        GrabSwing = AddComponentOfType<VillagerSwing>(gameObject);
    }

    T AddComponentOfType<T>(GameObject obj) where T : Component
    {
        if (obj.GetComponent<T>() == null)
        {
            obj.AddComponent<T>();
        }
        return obj.GetComponent<T>();
    }
}
