
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Bullet : SpaceObject
{
    [SerializeField] private SOMovementParameters movementParameters;

    private float elapsedTime;
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float lifetTime = 0;
    [SerializeField] private string tagNameForHit;
    public float damage;

    public GameObject bulletOwner;

    SpriteRenderer mainSpriteRenderer;

    private float sineDirection = 1f;

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
                break;
        }

        transform.position = currentPosition;
    }

    public void BulletInit(float _weaponDamage, GameObject owner, float direction)
    {
        damage = _weaponDamage;
        bulletOwner = owner;

        // Reset movement
        elapsedTime = 0f;
        lifetTime = 0f;
        startPosition = transform.position;
        targetPosition = ApplicationController.Instance.player.transform.position;

        sineDirection = direction;
    }

    protected void CheckVisibilityInCamera()
    {
        if (!CameraHelper.SpriteBoundsInCameraView(mainSpriteRenderer, Camera.main))
        {
            ReturnBullet();
            //ApplicationController.Instance.ObjectPool.Return(this);
            //Debug.Log(gameObject.name + " palautettu pooliin");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagNameForHit))
        {
            if (collision.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage(damage);
                ReturnBullet();
                //Debug.LogWarning("The enemy took " + damage + " damage");
            }

            else if (collision.TryGetComponent<Player>(out Player player))
            {
                player.TakeDamage(damage);
                ReturnBullet();
            }
        }
    }

    private void ReturnBullet()
    {
        bulletOwner = null;
        damage = 1f;
        ApplicationController.Instance.ObjectPool.Return(this);
    }
}
