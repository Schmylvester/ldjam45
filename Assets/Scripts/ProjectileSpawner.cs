using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public static ProjectileSpawner instance;
    public GameObject prefab;

    private void Start()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
        prefab.SetActive(false);
    }

    public void SpawnProjectile(Vector2 pos, string itemName, Vector2 direction, float damage, float speed, float range, bool firedByPlayer)
    {
        GameObject projectile = Instantiate(prefab);

        /*if (itemName == "Holy Bow")
        {
            projectile.GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.instance.getSprite(282);
        }
        else if (itemName == "Simple Bow")
        {
            projectile.GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.instance.getSprite(381);
        }
        else if (itemName == "Demonic Bow")
        {
            projectile.GetComponentInChildren<SpriteRenderer>().sprite = ItemDatabase.instance.getSprite(284);
        }*/

        projectile.GetComponent<Projectile>().speed = speed;
        projectile.GetComponent<Projectile>().direction = direction;
        projectile.GetComponent<Projectile>().damage = damage;
        projectile.GetComponent<Projectile>().range = range;
        projectile.GetComponent<Projectile>().firedByPlayer = firedByPlayer;
        projectile.transform.position = pos;
        projectile.GetComponent<Rigidbody2D>().position = pos;
        projectile.SetActive(true);
    }
}
