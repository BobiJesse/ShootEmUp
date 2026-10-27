
using System.Threading;
using UnityEngine;

public class Enemy : SpaceObject
{
    [SerializeField] private SOMovementParameters movementParameters;

    private float elapsedTime;
    private Vector2 startPosition;
    public Vector2 targetPosition;
    private Vector3 targetPositionOffSet = new Vector3 (0, 1, 0);
    private float lifeTime = 0;

    public float maxHealth = 2;
    public float health;

    [SerializeField] private int damage = 1;

    public Bullet bulletPrefab;

    public SpriteRenderer mainSpriteRenderer;

    private CancellationTokenSource shootingCancellation;

    private float sineDirection = 1f;

    public AudioClip dieSound;

    public GameObject explosionPrefab;
    public GameObject sineWeaponUpgradePrefab;
    public GameObject straightWeaponUpgradePrefab;

    private void Awake()
    {
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        startPosition = transform.position;
        //targetPosition = ApplicationController.Instance.player.transform.position + targetPositionOffSet;
        health = maxHealth;

        shootingCancellation = new CancellationTokenSource();
        _ = NewGameSequence(shootingCancellation.Token);
    }

    public void EnemyInit()
    {
        startPosition = transform.position;
        sineDirection = Random.value < 0.5f ? -1f : 1f;
        health = maxHealth;
        elapsedTime = 0f;
        targetPosition = (Vector2)transform.position + Vector2.down * 3f;
        //targetPosition = ApplicationController.Instance.player.transform.position + targetPositionOffSet;
    }

    private async Awaitable NewGameSequence(CancellationToken token)
    {
        try
        {
            while (true)
            {
                await Awaitable.WaitForSecondsAsync(1f, token);

                switch(movementParameters.Pattern)
                {
                    case SOMovementParameters.MovePattern.Straight:
                        ShootTowardsPlayer();
                        break;

                    case SOMovementParameters.MovePattern.Sine:
                    case SOMovementParameters.MovePattern.Target:
                        Shoot();
                        break;
                }
            }
        }
        catch (System.OperationCanceledException)
        {
            // Shooting was cancelled.
        }
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > 3)
        {
            lifeTime = 0;
            CheckVisibilityInCamera();
        }

        Vector2 currentPosition = transform.position;

        switch (movementParameters.Pattern)
        {
            case SOMovementParameters.MovePattern.Straight:
                currentPosition = MovementPatterns.Straight(
                    currentPosition,
                    movementParameters,
                    Time.deltaTime
                );
                break;

            case SOMovementParameters.MovePattern.Sine:
                elapsedTime += Time.deltaTime;

                currentPosition = MovementPatterns.Sine(
                    startPosition,
                    movementParameters,
                    elapsedTime,
                    sineDirection
                );
                break;

            case SOMovementParameters.MovePattern.Target:
                elapsedTime += Time.deltaTime;

                currentPosition = MovementPatterns.MoveToTarget(
                    startPosition,
                    targetPosition,
                    elapsedTime,
                    movementParameters
                );

                if (elapsedTime >= movementParameters.duration)
                {
                    startPosition = currentPosition;
                    targetPosition = ApplicationController.Instance.player.transform.position;
                    elapsedTime = 0f;
                }

                break;

            case SOMovementParameters.MovePattern.Hover:
                elapsedTime += Time.deltaTime;

                currentPosition = MovementPatterns.MoveToTarget(
                    startPosition,
                    targetPosition,
                    elapsedTime,
                    movementParameters
                );
                break;
        }

        transform.position = currentPosition;
    }

    private void Shoot()
    {
        Bullet b = ApplicationController.Instance.ObjectPool.Get<Bullet>(bulletPrefab as Bullet);

        b.BulletInit(damage, gameObject, sineDirection);
        b.transform.position = gameObject.transform.position;
    }

    private void ShootTowardsPlayer()
    {
        Bullet b = ApplicationController.Instance.ObjectPool.Get<Bullet>(bulletPrefab);

        b.transform.position = transform.position;
        b.BulletInitTowardsPlayer(damage, gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        AudioManager.instance.PlaySFX(dieSound);
        //ApplicationController.Instance.ObjectPool.Return(this);
        ReturnEnemy();
        //Debug.Log(gameObject.name + " palautettu pooliin");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        int dropChance = Random.Range(0, 10);

        if (dropChance == 2)
        {
            Instantiate(straightWeaponUpgradePrefab, transform.position, Quaternion.identity);
        }
        else if (dropChance == 3)
        {
            Instantiate(sineWeaponUpgradePrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }

    protected void CheckVisibilityInCamera()
    {
        if (!CameraHelper.SpriteBoundsInCameraView(mainSpriteRenderer, Camera.main))
        {
            //ApplicationController.Instance.ObjectPool.Return(this);
            ReturnEnemy();
            //Debug.Log(gameObject.name + " palautettu pooliin");
        }
    }

    private void OnDisable()
    {
        if (shootingCancellation != null)
        {
            shootingCancellation.Cancel();
            shootingCancellation.Dispose();
            shootingCancellation = null;
        }
    }

    private void ReturnEnemy()
    {
        ApplicationController.Instance.ObjectPool.Return(this);
        health = maxHealth;
    }
}
