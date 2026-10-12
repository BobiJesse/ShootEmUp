using System;
using UnityEngine;

public class Collectible : SpaceObject
{
    SpriteRenderer renderer;

    public enum collectibleType
    {
        coin,
        health,
        upgrade
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
        Debug.Log("collectible");
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            Debug.Log("moi");
        }
    }
}
