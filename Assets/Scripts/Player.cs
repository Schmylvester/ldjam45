using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour
    {
        struct FrameData
        {
            public Vector2 moveDirection;
            public bool interacted;
            public bool attack;
        };

        enum Facing
        {
            Up,
            Down,
            Left,
            Right
        }

        FrameData lastFrame;
        FrameData thisFrame;

        PlayerStats stats;
        Facing facing = Facing.Up;

        bool attacking = false;

        void Start()
        {
            lastFrame.moveDirection = Vector2.zero;
            lastFrame.interacted = false;
            lastFrame.attack = false;

            thisFrame.moveDirection = Vector2.zero;
            thisFrame.interacted = false;
            thisFrame.attack = false;

            GetComponentInChildren<AnimationStateController>().SetState("IdleUp");
            stats = GetComponent<PlayerStats>();
        }

        void HandleInput()
        {
            lastFrame = thisFrame;

            thisFrame.moveDirection = Vector2.zero;
            thisFrame.interacted = false;
            thisFrame.attack = false;

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

            if (Input.GetMouseButton(0))
            {
                thisFrame.attack = true;
            }
        }

        void Move()
        {
            Vector2 movement = thisFrame.moveDirection * stats.GetActualMovespeed() * Time.fixedDeltaTime;
            GetComponent<Rigidbody2D>().velocity = movement;
        }

        void Animate()
        {
            if (lastFrame.moveDirection.y != 1 &&
                thisFrame.moveDirection.y == 1)
            {
                facing = Facing.Up;
                GetComponentInChildren<AnimationStateController>().SetState("WalkUp");
            }
            else if (lastFrame.moveDirection.y != -1 && 
                thisFrame.moveDirection.y == -1)
            {
                facing = Facing.Down;
                GetComponentInChildren<AnimationStateController>().SetState("WalkDown");
            }
            else if (lastFrame.moveDirection.x != -1 &&
                thisFrame.moveDirection.x == -1)
            {
                facing = Facing.Left;
                GetComponentInChildren<AnimationStateController>().SetState("WalkLeft");
            }
            else if (lastFrame.moveDirection.x != 1 &&
                thisFrame.moveDirection.x == 1)
            {
                facing = Facing.Right;
                GetComponentInChildren<AnimationStateController>().SetState("WalkRight");
            }

            if (thisFrame.moveDirection == Vector2.zero)
            {
                if (lastFrame.moveDirection.x == -1)
                {
                    facing = Facing.Left;
                    GetComponentInChildren<AnimationStateController>().SetState("IdleLeft");
                }
                else if (lastFrame.moveDirection.x == 1)
                {
                    facing = Facing.Right;
                    GetComponentInChildren<AnimationStateController>().SetState("IdleRight");
                }
                else if (lastFrame.moveDirection.y == -1)
                {
                    facing = Facing.Down;
                    GetComponentInChildren<AnimationStateController>().SetState("IdleDown");
                }
                else if (lastFrame.moveDirection.y == 1)
                {
                    facing = Facing.Up;
                    GetComponentInChildren<AnimationStateController>().SetState("IdleUp");
                }
            }
        }

        void Update()
        {
            HandleInput();
            Animate();
            Attack();
        }

        void Attack()
        {
            if (thisFrame.attack && !attacking)
            {
                StartCoroutine(DoAttack());
            }
        }

        IEnumerator DoAttack()
        {
            transform.GetChild(2).gameObject.SetActive(true);
            attacking = true;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Vector2 facingDir = Vector2.zero;
            if (facing == Facing.Left)
            {
                facingDir.x = -1;
            }
            else if (facing == Facing.Right)
            {
                facingDir.x = 1;
            }
            else if (facing == Facing.Up)
            {
                facingDir.y = 1;
            }
            else if (facing == Facing.Down)
            {
                facingDir.y = -1;
            }

            transform.GetChild(0).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
            transform.GetChild(1).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
            transform.GetChild(2).transform.Translate(new Vector3(facingDir.x * 0.2f, facingDir.y * 0.2f, 0), Space.Self);
            yield return new WaitForSeconds(0.2f);
            transform.GetChild(0).transform.localPosition = Vector3.zero;
            transform.GetChild(1).transform.localPosition = Vector3.zero;
            transform.GetChild(2).transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
            transform.GetChild(2).gameObject.SetActive(false);
            attacking = false;
            yield return null;
        }

        private void FixedUpdate()
        {
            if (!attacking)
            {
                Move();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        private void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log(collider.transform.parent);
            if (collider.transform.parent)
            {
                if (collider.transform.parent.name == "Monster") //todo: tag/layer
                {
                    collider.transform.parent.GetComponent<Monster>().OnHit(stats.GetActualDamage());
                }
            }
        }
    }
}