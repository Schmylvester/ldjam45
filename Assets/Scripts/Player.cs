using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 moveDirection;
    float moveSpeed = 2;

    void Start()
    {

    }

    void Update()
    {
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.y += 1;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.y -= 1;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1;
        }

        Vector2 movement = moveDirection * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void FixedUpdate()
    {
    }
}
