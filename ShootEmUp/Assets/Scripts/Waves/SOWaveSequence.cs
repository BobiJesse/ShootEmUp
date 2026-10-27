using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SOWaveSequence", menuName = "Scriptable Objects/SOWaveSequence")]
public class SOWaveSequence : ScriptableObject
{
    [SerializeField] private List<EnemyWave> waves;

    public List<EnemyWave> Waves => waves;
}
