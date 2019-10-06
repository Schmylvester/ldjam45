using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    public float damage;
    public float range;
    public bool firedByPlayer;

    Vector2 spawnPos;
    private void Start()
    {
        spawnPos = transform.position;
        GetComponent<Rigidbody2D>().transform.LookAt(Vector3.forward, direction);
        Destroy(gameObject, 10);
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = direction * speed * Time.fixedDeltaTime;
    }

    private void Update()
    {
        Vector2 vectorToSpawn = spawnPos - (Vector2)transform.position;
        float distance = Mathf.Abs(vectorToSpawn.magnitude);
        if (distance > range / 100.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (firedByPlayer)
        {
            if (collision.transform.tag == "Enemy")
            {
                collision.transform.GetComponent<Monster>().OnHit(damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.transform.tag == "Player")
            {
                collision.transform.GetComponent<Player.Player>().OnHit(damage);
                Destroy(gameObject);
            }
        }
    }
}
