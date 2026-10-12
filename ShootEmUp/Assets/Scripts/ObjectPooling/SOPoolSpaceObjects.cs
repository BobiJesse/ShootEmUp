using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOPoolSpaceObjects", menuName = "Scriptable Objects/SOPoolSpaceObjects")]
public class SOPoolSpaceObjects : ScriptableObject
{
    [SerializeField]
    public List<PoolEntry> poolObjects;
}
