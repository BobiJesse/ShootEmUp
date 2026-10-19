using UnityEngine;
using UnityEngine.InputSystem;

public class Player : SpaceObject
{
    public float health = 5;

    public AudioClip deathSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            //game over
            Debug.Log("Game Over");
            Die();
        }
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(deathSound);
    }
}
