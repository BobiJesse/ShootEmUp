using UnityEngine;

public class Collectible : SpaceObject
{
    SpriteRenderer renderer;
    [SerializeField] private SOWeapon weaponToGive;

    public enum collectibleType
    {
        coin,
        health,
        upgrade,
        straightWeapon,
        sineWeapon
    }

    public collectibleType chosenType;

    public void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (chosenType == collectibleType.coin)
        {
            renderer.color = Color.yellow;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player == null)
                return;

            Destroy(gameObject);

            switch (chosenType)
            {
                case collectibleType.coin:
                    break;

                case collectibleType.health:
                    break;

                case collectibleType.straightWeapon:
                    if (player.CheckWeapon(weaponToGive))
                        player.UpgradeWeapon();
                    else
                        player.ChangeWeapon(weaponToGive);
                    break;

                case collectibleType.sineWeapon:
                    if (player.CheckWeapon(weaponToGive))
                        player.UpgradeWeapon();
                    else
                        player.ChangeWeapon(weaponToGive);
                    break;
            }
        }
    }
}
