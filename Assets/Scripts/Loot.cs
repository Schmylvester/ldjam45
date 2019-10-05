using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public bool moveTowardsPlayer = false;
    public GameObject player;
    public float acceleration = 2;
    public float velocity = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
            Destroy(gameObject);
            //todo: something with the player
        }
    }
}
