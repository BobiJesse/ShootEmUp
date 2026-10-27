using System;
using UnityEngine;

[Serializable]
public class PoolEntry
{
    public SpaceObject prefab;
    public int initialSize = 10;
    public Color32 colorID = new Color(0, 0, 0, 255);
}
