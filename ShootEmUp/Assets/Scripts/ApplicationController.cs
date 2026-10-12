using Unity.VisualScripting;
using UnityEngine;

public class ApplicationController : SingletonMono<ApplicationController>
{
    public PlayerController player { get; private set; }
    private UIController ui;

    [HideInInspector]
    public SOPoolSpaceObjects PoolSpaceObjects;
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
        //Then initialize pool(s) with amount of initialSize
        InitObjectPool();

        ui = Instantiate(Resources.Load("UI")).GetComponent<UIController>().Init();
        player = Instantiate<PlayerController>(Resources.Load<PlayerController>("PlayersShip"));
    }

    public void StartNewGame()
    {
        Awaitable awaitable = NewGameSequence();
        //awaitable.Cancel(); <-- jos awaitable tarvitsee lopettaa, se voidaan tehdä Cancel-metodilla (tai tokenilla, mutta tämä on helpompi tapa)
    }
    private async Awaitable NewGameSequence()
    {
        for (int i = 3; i > 0; i--)
        {
            Debug.Log(i + "...");
            await Awaitable.WaitForSecondsAsync(1f);
        }
        //This is for testing purposes only! Later on we'll spawn waves of enemies with more sophisticated way
        SpawnTestEnemies(6);
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
        }
    }
}