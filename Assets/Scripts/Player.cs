using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    struct PerFrame
    {
        public Vector2 moveDirection;
        public bool interacted;
    };

    PerFrame lastFrame;
    PerFrame thisFrame;

    float moveSpeed = 100;

    void Start()
    {
        lastFrame.moveDirection = Vector2.zero;
        lastFrame.interacted = false;
        thisFrame.moveDirection = Vector2.zero;
        thisFrame.interacted = false;

        GetComponentInChildren<AnimationStateController>().SetState("IdleDown");
    }

    void HandleInput()
    {
        lastFrame.moveDirection = thisFrame.moveDirection;
        thisFrame.moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            thisFrame.moveDirection.y += 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            thisFrame.moveDirection.x -= 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            thisFrame.moveDirection.y -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            thisFrame.moveDirection.x += 1;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            thisFrame.interacted = true;
        }
    }

    void Move()
    {
        Vector2 movement = thisFrame.moveDirection * moveSpeed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    void Animate()
    {
        if (lastFrame.moveDirection.y != 1 &&
            thisFrame.moveDirection.y == 1)
        {
            GetComponentInChildren<AnimationStateController>().SetState("WalkUp");
        }
        else if (lastFrame.moveDirection.y != -1 && 
            thisFrame.moveDirection.y == -1)
        {
            GetComponentInChildren<AnimationStateController>().SetState("WalkDown");
        }
        else if (lastFrame.moveDirection.x != -1 &&
            thisFrame.moveDirection.x == -1)
        {
            GetComponentInChildren<AnimationStateController>().SetState("WalkLeft");
        }
        else if (lastFrame.moveDirection.x != 1 &&
            thisFrame.moveDirection.x == 1)
        {
            GetComponentInChildren<AnimationStateController>().SetState("WalkRight");
        }

        if (thisFrame.moveDirection == Vector2.zero)
        {
            if (lastFrame.moveDirection.x == -1)
            {
                GetComponentInChildren<AnimationStateController>().SetState("IdleLeft");
            }
            else if (lastFrame.moveDirection.x == 1)
            {
                GetComponentInChildren<AnimationStateController>().SetState("IdleRight");
            }
            else if (lastFrame.moveDirection.y == -1)
            {
                GetComponentInChildren<AnimationStateController>().SetState("IdleDown");
            }
            else if (lastFrame.moveDirection.y == 1)
            {
                GetComponentInChildren<AnimationStateController>().SetState("IdleUp");
            }
        }
    }

    void Update()
    {
        HandleInput();
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
    }
}
