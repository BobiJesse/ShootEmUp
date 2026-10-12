using Unity.VisualScripting;
using UnityEngine;

public class ApplicationController : SingletonMono<ApplicationController>
{
    private PlayerController player;
    private UIController ui;

    [RuntimeInitializeOnLoadMethod]
    static void OnInit()
    {
        Instance.Init();
    }

    public void Init()
    {
        ui = Instantiate(Resources.Load("UI")).GetComponent<UIController>().Init();
        player = Instantiate<PlayerController>(Resources.Load<PlayerController>("PlayersShip"));
    }

    public void StartNewGame()
    {

    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
    Application.Quit();
    #endif
    }
}