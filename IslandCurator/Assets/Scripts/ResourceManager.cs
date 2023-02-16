using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] int _numResources = 4;
    [SerializeField] int _maxResources = 9999;

    List<int> _resourceCounts = new List<int>();
    
    public enum ResourceType {Fish, Meat, Wheat, Wood}

    void Start()
    {
        for (int i = 0; i < _numResources; i++)
        {
            _resourceCounts.Add(0);
        }
    }
    
    public void AddResource(ResourceType type, int count)
    {
        int index = _numResources + 1;
        switch (type)
        {
            case ResourceType.Fish:
                index = 0;
                break;
            case ResourceType.Meat:
                index = 1;
                break;
            case ResourceType.Wheat:
                index = 2;
                break;
            case ResourceType.Wood:
                index = 3;
                break;
        }

        _resourceCounts[index] = Mathf.Clamp(_resourceCounts[index] + count, 0, _maxResources);
    }
}
