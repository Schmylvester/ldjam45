using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private GameObject player;
    
    enum State
    {
        Idle,
        Patrol,
        Death,
        Attacking,
        Defending,
        Hunting
    }
    
    [SerializeField] State state = State.Idle;
    Vector2 vectorToPlayer;
    float distanceToPlayer = 9999;
    float moveSpeed = 30;
    [SerializeField] bool playerInLineOfSight = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void CheckState()
    {
        if (state == State.Idle)
        {
            if (distanceToPlayer < 1 &&
                playerInLineOfSight)
            {
                state = State.Hunting;
            }
        }
        else if (state == State.Hunting)
        {
            if (distanceToPlayer > 1.5 ||
                !playerInLineOfSight)
            {
                state = State.Idle;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (distanceToPlayer < 0.25 &&
                playerInLineOfSight)
            {
                state = State.Attacking;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (state == State.Attacking)
        {
            if (distanceToPlayer > 0.3)
            {
                state = State.Hunting;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    private void Update()
    {
        vectorToPlayer = player.transform.position - transform.position;
        distanceToPlayer = Mathf.Abs(vectorToPlayer.magnitude);

        Vector3 offset = new Vector3(-0.1f, 0, 0);
        bool terrainBlocking = false;
        for (int i = 0; i < 20; ++i)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Terrain"));
            Physics2D.Raycast(transform.position + offset, new Vector3(vectorToPlayer.x - offset.x, vectorToPlayer.y, 0), filter, hits, 100);
            Debug.DrawRay(transform.position + offset, new Vector3(vectorToPlayer.x - offset.x, vectorToPlayer.y, 0));
            offset.x += 0.01f;
            if (hits[0])
            {
                terrainBlocking = true;
                Debug.Log(hits[0].transform.parent.name);
            }
        }
        playerInLineOfSight = !terrainBlocking;

        CheckState();

        switch(state)
        {
            case State.Idle: IdleUpdate(); break;
            case State.Patrol: PatrolUpdate(); break;
            case State.Death: DeathUpdate(); break;
            case State.Attacking: AttackingUpdate(); break;
            case State.Defending: DefendingUpdate(); break;
            case State.Hunting: HuntingUpdate(); break;
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            //case State.Idle: IdleUpdate(); break;
            //case State.Patrol: PatrolUpdate(); break;
            //case State.Death: DeathUpdate(); break;
            //case State.Attacking: AttackingUpdate(); break;
            //case State.Defending: DefendingUpdate(); break;
            case State.Hunting: HuntingFixedUpdate(); break;
            default: break;
        }
    }

    void IdleUpdate()
    {

    }

    void PatrolUpdate()
    {

    }

    void DeathUpdate()
    {

    }

    void AttackingUpdate()
    {

    }

    void DefendingUpdate()
    {

    }

    void HuntingUpdate()
    {
    }

    void HuntingFixedUpdate()
    {
        Vector2 movement = vectorToPlayer.normalized * moveSpeed * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = movement;
    }
}
