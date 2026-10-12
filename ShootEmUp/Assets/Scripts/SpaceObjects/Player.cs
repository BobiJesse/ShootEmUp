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
        Vector3 mousePosition = Mouse.current.position.ReadValue();

        // Convert mouse position from screen space to world space
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Direction from player to mouse
        Vector2 direction = mousePosition - transform.position;

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Rotate the spaceship
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
}
