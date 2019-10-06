using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTiler : MonoBehaviour
{
    [SerializeField] GameObject sprite;
    [SerializeField] Sprite floorTile;
    [SerializeField] Sprite lowerWallTile;
    [SerializeField] Sprite upperWallTile;
    [SerializeField] int width;
    [SerializeField] int height;

    private void Start()
    {
        GameObject instance;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                instance = Instantiate(sprite, new Vector3(x, y), new Quaternion(), transform);
                instance.transform.position /= 3.125f;
                instance.GetComponent<SpriteRenderer>().sprite = floorTile;
            }
            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(x, -1);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = upperWallTile;

            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(x, -2);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = lowerWallTile;

            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(x, height);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = upperWallTile;

            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(x, height - 1);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = lowerWallTile;
        }

        for (int y = 0; y < height; ++y)
        {
            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(-1, y);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = upperWallTile;
            instance = Instantiate(sprite, transform);
            instance.transform.position = new Vector3(width, y);
            instance.transform.position /= 3.125f;
            instance.GetComponent<SpriteRenderer>().sprite = upperWallTile;
        }
    }
}
