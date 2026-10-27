using Unity.VisualScripting;
using UnityEngine;

public class ApplicationController : SingletonMono<ApplicationController>
{
    public PlayerController player { get; private set; }
    private UIController ui;

    [HideInInspector]
    public SOPoolSpaceObjects PoolSpaceObjects;
    public SOWaveSequence waveSequence;

    public EnemyWavePlayer wavePlayer;
    public EnemyWaveManager waveManager;

    [System.NonSerialized]
    public ObjectPool ObjectPool = new ObjectPool();

    [RuntimeInitializeOnLoadMethod]
    static void OnInit()
    {
        Instance.Init();
    }

    public void Init()
    {
        //Load Scriptable object with a list of prefab references
        PoolSpaceObjects = Resources.Load<SOPoolSpaceObjects>("PoolSpaceObjects");

        waveSequence = Resources.Load<SOWaveSequence>("WaveSequence");
        //Then initialize pool(s) with amount of initialSize
        InitObjectPool();

        wavePlayer = new EnemyWavePlayer(ObjectPool, Camera.main, PoolSpaceObjects);
        waveManager = new EnemyWaveManager(waveSequence, wavePlayer);

        ui = Instantiate(Resources.Load("UI")).GetComponent<UIController>().Init();
        player = Instantiate<PlayerController>(Resources.Load<PlayerController>("PlayersShip"));
    }

    public void StartNewGame()
    {
        Awaitable awaitable = waveManager.StartWaves();
        //Awaitable awaitable = NewGameSequence();
        //awaitable.Cancel(); <-- jos awaitable tarvitsee lopettaa, se voidaan tehdä Cancel-metodilla (tai tokenilla, mutta tämä on helpompi tapa)
    }
    private async Awaitable NewGameSequence()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 3; j > 0; j--)
            {
                Debug.Log(j + "...");
                await Awaitable.WaitForSecondsAsync(1f);
            }
                SpawnTestEnemies(6);
            await Awaitable.WaitForSecondsAsync(5f);
        }
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
    }
    
    private void InitObjectPool()
    {
        foreach (PoolEntry entry in PoolSpaceObjects.poolObjects)
        {
            ObjectPool.Register(entry.prefab, entry.initialSize);
        }
    }

    private void SpawnTestEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = ObjectPool.Get(PoolSpaceObjects.poolObjects[Random.Range(0, 3)].prefab as Enemy);

            enemy.transform.position =
                CameraHelper.GetSpawnPositionAbove(
                    enemy.mainSpriteRenderer,
                    Camera.main,
                    i,
                    count);

            enemy.EnemyInit();
        }
    }
}