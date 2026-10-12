using System;
using UnityEngine;

[Serializable]
public class PoolEntry
{
    public SpaceObject prefab;
    public int initialSize = 10;
}
