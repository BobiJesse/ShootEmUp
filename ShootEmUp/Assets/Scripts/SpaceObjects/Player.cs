using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SpaceObject
{
    public int health = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {
        health -= 1;

        if (health <= 0)
        {
            //game over
            Debug.Log("Game Over");
        }
    }

    public void Die()
    {

    }
}
