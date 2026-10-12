
using UnityEngine;

public class Bullet : SpaceObject
{
    [SerializeField] private SOMovementParameters movementParameters;

    private float elapsedTime;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float lifetTime = 0;
    [SerializeField] private string tagNameForHit;
    public int damage;

    SpriteRenderer mainSpriteRenderer;

    private void Start()
    {
        startPosition = transform.position;
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        lifetTime += Time.deltaTime;

        if (lifetTime > 3)
        {
            lifetTime = 0;
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
                break;
        }

        transform.position = currentPosition;
    }

    public void BulletInit(int _weaponDamage)
    {
        damage = _weaponDamage;
    }

    protected void CheckVisibilityInCamera()
    {
        if (!CameraHelper.SpriteBoundsInCameraView(mainSpriteRenderer, Camera.main))
        {
            ApplicationController.Instance.ObjectPool.Return(this);
            Debug.Log(gameObject.name + " palautettu pooliin");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagNameForHit))
        {
            ApplicationController.Instance.ObjectPool.Return(this);
            if (collision.CompareTag("Enemy"))
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                //ApplicationController.Instance.ObjectPool.Return(collision.gameObject.GetComponent<Enemy>());

            if (collision.CompareTag("Player"))
                collision.gameObject.GetComponent<Player>().TakeDamage();
        }
    }
}
