using System;
using UnityEngine;

[Serializable]

public class EnemyWave
{
    public Texture2D waveTexture;
    public float rowInterval = 0.5f;
    public float delayBeforeWave = 3f;
}
