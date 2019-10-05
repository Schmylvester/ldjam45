using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGround : MonoBehaviour
{
    [SerializeField] Sprite[] groundTiles = null;
    [SerializeField] GameObject groundPrefab = null;
    [SerializeField] int width = 0;
    [SerializeField] int height = 0;

    private void Start()
    {
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                SpriteRenderer groundTile = Instantiate(groundPrefab, transform).GetComponent<SpriteRenderer>();
                groundTile.sprite = groundTiles[Random.Range(0, groundTiles.Length)];
                groundTile.transform.localPosition += new Vector3(x, y);

            }
        }
    }
}
