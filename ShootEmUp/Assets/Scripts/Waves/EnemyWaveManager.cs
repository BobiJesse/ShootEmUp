using UnityEngine;

public class EnemyWaveManager
{
    private readonly SOWaveSequence sequence;
    private readonly EnemyWavePlayer wavePlayer;
    private int currentWaveIndex;

    public int currentWave => currentWaveIndex + 1;

    public EnemyWaveManager(SOWaveSequence sequence, EnemyWavePlayer wavePlayer)
    {
        this.sequence = sequence;
        this.wavePlayer = wavePlayer;
    }

    public async Awaitable StartWaves()
    {
        Debug.Log("startwaves called");
        Debug.Log($"Sequence null: {sequence == null}");
        Debug.Log($"Wave count: {sequence.Waves?.Count}");

        if (sequence.Waves == null || sequence.Waves.Count == 0)
        {
            Debug.Log("sequence waves count 0, returning");
            return;
        }

        while (true)
        {
            Debug.Log("inside while");
            EnemyWave wave = sequence.Waves[currentWaveIndex];

            await Awaitable.WaitForSecondsAsync(wave.delayBeforeWave);

            await wavePlayer.PlayWave(wave);

            currentWaveIndex = (currentWaveIndex + 1) % sequence.Waves.Count;
        }
    }
}
