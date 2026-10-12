using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Dictionary<SpaceObject, Queue<SpaceObject>> pools = new();
    private Dictionary<SpaceObject, SpaceObject> objectPrefabs = new();

    public void Register<T>(T prefab, int initialSize = 10) where T : SpaceObject
    {
        if (pools.ContainsKey(prefab))
        {
            Debug.LogWarning($"Trying to register prefab {prefab.name} multiple times. Check your SOPoolObjects list for duplicates.");
            return;
        }

        Queue<SpaceObject> pool = new();

        pools.Add(prefab, pool);

        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
            objectPrefabs.Add(obj, prefab);
        }
    }

    public T Get<T>(T prefab) where T : SpaceObject
    {
        if (!pools.TryGetValue(prefab, out Queue<SpaceObject> pool))
        {
            Debug.LogError($"Prefab {prefab.name} is not registered.");
            return null;
        }

        T obj;

        if (pool.Count > 0)
        {
            obj = (T)pool.Dequeue();
        }
        else
        {
            obj = Object.Instantiate(prefab);
            obj.gameObject.SetActive(false);
            objectPrefabs.Add(obj, prefab);
        }

        //Set object active again:
        obj.gameObject.SetActive(true);
        //Call SpaceObject's "Init" method to initialize it's behaviour e.g. start movement, shooting, first animation play
        obj.Init();

        return obj;
    }

    public void Return<T>(T obj) where T : SpaceObject
    {
        if (obj == null)
            return;

        if (!objectPrefabs.TryGetValue(obj, out SpaceObject prefab))
        {
            Debug.LogWarning(
                $"Object {obj.name} does not belong to this pool.");

            return;
        }

        //Call SpaceObject's "ResetObject" method to reset e.g. timers, animations or other behaviours that need to stop
        obj.ResetObject();
        obj.gameObject.SetActive(false);

        pools[prefab].Enqueue(obj);
    }
}