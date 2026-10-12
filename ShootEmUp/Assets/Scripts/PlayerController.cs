using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SpaceObject
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float moveForce = 10f;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private Rigidbody2D rb;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Jump.performed += OnFire;

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);

        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (controls.Player.Boost.IsPressed())
        {
            rb.AddForce(transform.up * moveForce);
        }
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        Debug.Log("Pew!");
        // Spawn laser here
        AudioController.Instance.PlaySoundEffect("Laser_Shoot_1");
    }
}