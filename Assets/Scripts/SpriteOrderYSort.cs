using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrderYSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Transform transformWithY;
    public int orderOffset = 0;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!spriteRenderer)
        {
            Debug.LogError("You need to assign this to an object with a sprite renderer");
        }

        if (!transformWithY)
        {
            Debug.LogError("You need to assign a transform who's y will be used");
        }
    }

    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = (int)(transform.position.y * -1000) + orderOffset;
    }
}
