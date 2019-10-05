using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public bool moveTowardsPlayer = false;
    public GameObject player;
    public float acceleration = 2;
    public float velocity = 0;
    public string itemName = "Mushroom";

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (ItemDatabase.instance != null)
        {
            Item item = ItemDatabase.instance.getItem(itemName);
            if (item.name != new Item().name)
            {
                GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.instance.getSprite(item.spriteIdx);
            }
            else
            {
                Debug.LogError("Could not find item " + itemName + " in database");
            }
        }
    }

    private void Update()
    {
        if (moveTowardsPlayer)
        {
            MoveTowardsPlayer();
        }
        else
        {
            float distanceToPlayer = player.transform.position.magnitude - transform.position.magnitude;
            if (Mathf.Abs(distanceToPlayer) < 1.0f) //todo: player magnet pickup range? make it variable
            {
                moveTowardsPlayer = true;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.gameObject.tag == "Player")
        {
            //todo: player inventory singleton
            //Item item = ItemDatabase.instance.getItem(itemName);
            //Player.PlayerInventory.instance.addItem(item);
            Destroy(gameObject);
        }
    }
}
