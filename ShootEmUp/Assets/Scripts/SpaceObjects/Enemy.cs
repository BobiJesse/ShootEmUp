
using System.Threading;
using UnityEngine;

public class Enemy : SpaceObject
{
    [SerializeField] private SOMovementParameters movementParameters;

    private float elapsedTime;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private Vector3 targetPositionOffSet = new Vector3 (0, 1, 0);
    private float lifeTime = 0;

    public int maxHealth = 2;
    public int health;

    public Bullet bulletPrefab;

    public SpriteRenderer mainSpriteRenderer;

    private CancellationTokenSource shootingCancellation;

    private void Awake()
    {
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPosition = transform.position;
        targetPosition = ApplicationController.Instance.player.transform.position + targetPositionOffSet;
        health = maxHealth;

        shootingCancellation = new CancellationTokenSource();
        _ = NewGameSequence(shootingCancellation.Token);
    }

    private async Awaitable NewGameSequence(CancellationToken token)
    {
        try
        {
            while (true)
            {
                await Awaitable.WaitForSecondsAsync(1f, token);

                Shoot();
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
                    elapsedTime
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
        }

        transform.position = currentPosition;
    }

    private void Shoot()
    {
        Bullet b = ApplicationController.Instance.ObjectPool.Get<Bullet>(bulletPrefab as Bullet);

        b.transform.position = gameObject.transform.position;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        ApplicationController.Instance.ObjectPool.Return(this);
        Debug.Log(gameObject.name + " palautettu pooliin");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage();
        }
    }

    protected void CheckVisibilityInCamera()
    {
        if (!CameraHelper.SpriteBoundsInCameraView(mainSpriteRenderer, Camera.main))
        {
            ApplicationController.Instance.ObjectPool.Return(this);
            Debug.Log(gameObject.name + " palautettu pooliin");
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
}
