using UnityEngine;

public class EnemyWavePlayer
{
    private readonly ObjectPool objectPool;
    private readonly Camera camera;
    private readonly SOPoolSpaceObjects poolSpecs;

    public EnemyWavePlayer(ObjectPool objectPool, Camera camera, SOPoolSpaceObjects poolSpecs)
    {
        this.objectPool = objectPool;
        this.camera = camera;
        this.poolSpecs = poolSpecs;
    }

    public async Awaitable PlayWave(EnemyWave wave)
    {
        Texture2D texture = wave.waveTexture;
        Color32[] pixels = texture.GetPixels32();


        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color32 color = pixels[y * texture.width + x];
                SpaceObject prefab = FindPrefab(color);

                //Debug.Log($"Columns Checked {x} on row {y}");

                if (prefab == null)
                {
                    continue;
                }

                SpaceObject obj = objectPool.Get(prefab);
                Enemy enemy = obj as Enemy;
                enemy.transform.position = CameraHelper.GetSpawnPositionAbove(enemy.mainSpriteRenderer, camera, x, texture.width);
                enemy.EnemyInit();
            }
            await Awaitable.WaitForSecondsAsync(wave.rowInterval);
            //Debug.Log($"Rows checked {y}");
        }
    }

    private SpaceObject FindPrefab(Color32 color)
    {
        if (color.r == 0 && color.g == 0 && color.b == 0)
        {
            return null;
        }

        foreach (PoolEntry entry in poolSpecs.poolObjects)
        {
            Color32 id = entry.colorID;
            if (color.r == id.r && color.g == id.g && color.b == id.b)
            {
                return entry.prefab;
            }
        }
        Debug.LogWarning($"Unknown spawn color: {color}");
        return null;
    }
}
