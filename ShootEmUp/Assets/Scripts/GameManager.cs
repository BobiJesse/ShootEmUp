using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    private void Awake()
    {
        //instance check
        if (instance == null)
        {
            instance = this;
        }
        //destroy others if one already exists
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
