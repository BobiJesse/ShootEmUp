using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SpaceObject
{
    [SerializeField] private SOWeapon weaponSpecs;

    private Weapon currentWeapon;
    private float nextFireTime;

    [SerializeField] private float moveSpeed = 6f;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private Rigidbody2D rb;

    private bool sineGoesLeft = true;

    private void Awake()
    {
        controls = new InputSystem_Actions();

        controls.Player.Jump.performed += OnFire;

        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;

        rb = GetComponent<Rigidbody2D>();

        currentWeapon = new Weapon(weaponSpecs);
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

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + 1f / currentWeapon.FireSpeed;

        float damage = currentWeapon.Damage;

        Debug.Log("Pew! Damage: " + damage);

        //AudioController.Instance.PlaySoundEffect("Laser_Shoot_1");

        //Instantiate(weaponSpecs.bulletPrefab, transform.position + transform.up * 0.8f, transform.rotation);
        Bullet b = ApplicationController.Instance.ObjectPool.Get<Bullet>(weaponSpecs.bulletPrefab as Bullet);

        float direction = sineGoesLeft ? 1f : -1f;
        b.transform.position = gameObject.transform.position;
        b.BulletInit(damage, gameObject, direction);

        if (weaponSpecs.chosenWeaponStyle == SOWeapon.WeaponStyle.Sine)
        {
            sineGoesLeft = !sineGoesLeft;
        }
        //Debug.LogWarning("Bullet given " + damage + "damage");
    }

    public void ChangeWeapon(SOWeapon newWeapon)
    {
        weaponSpecs = newWeapon;
        currentWeapon = new Weapon(weaponSpecs);
        Debug.Log(newWeapon + "" + currentWeapon);
    }

    public void UpgradeWeapon()
    {
        currentWeapon.IncreaseLevel();
        Debug.Log("Weapon level increased to " + currentWeapon.CurrentLevel);
    }

    public bool CheckWeapon(SOWeapon newWeapon)
    {
        if (newWeapon == weaponSpecs)
            return true;
        else 
            return false;
    }
}