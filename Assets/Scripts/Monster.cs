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
    [SerializeField] bool playerInLineOfSight = false;
    public bool cat = false;
    public bool aggresive = true;

    PlayerStats stats;
    bool invulnerable = false;
    bool willAttackPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<PlayerStats>();
    }

    void CheckState()
    {
        if (state == State.Idle)
        {
            if (willAttackPlayer &&
                distanceToPlayer < 1 &&
                playerInLineOfSight)
            {
                state = State.Hunting;
                GetComponentInChildren<AnimationStateController>().SetState("WalkDown");
            }
        }
        else if (state == State.Hunting)
        {
            if (distanceToPlayer > 1.5 ||
                !playerInLineOfSight)
            {
                state = State.Idle;
                GetComponentInChildren<AnimationStateController>().SetState("WalkUp");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (distanceToPlayer < 0.20 &&
                playerInLineOfSight &&
                !cat)
            {
                state = State.Attacking;
                GetComponentInChildren<AnimationStateController>().SetState("Attack");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (state == State.Attacking)
        {
            if (distanceToPlayer > 0.25)
            {
                state = State.Hunting;
                GetComponentInChildren<AnimationStateController>().SetState("WalkLeft");
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (state == State.Defending)
        {
            StartCoroutine(Defending());
        }
    }

    IEnumerator Defending()
    {
        if (cat) yield return null;
        GetComponentInChildren<AnimationStateController>().SetState("Defend");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        state = State.Attacking;
        StartCoroutine(SpeedUpAfterDefending());
    }

    IEnumerator SpeedUpAfterDefending()
    {
        stats.moveSpeedModifier += 6;
        yield return new WaitForSeconds(0.4f);
        stats.moveSpeedModifier -= 6;
        yield return null;
    }

    private void Update()
    {
        if (GameObservables.gamePaused)
        {
            return;
        }

        willAttackPlayer = aggresive || stats.currentHealth < stats.GetActualMaxHealth();
        if (cat) willAttackPlayer = true;

        vectorToPlayer = player.transform.position - transform.position;
        distanceToPlayer = Mathf.Abs(vectorToPlayer.magnitude);

        Vector3 offset = new Vector3(-0.1f, 0, 0);
        bool terrainBlocking = false;
        for (int i = 0; i < 10; ++i)
        {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Terrain"));
            Physics2D.Raycast(transform.position + offset, new Vector3(vectorToPlayer.x - offset.x, vectorToPlayer.y, 0), filter, hits, 100);
            //Debug.DrawRay(transform.position + offset, new Vector3(vectorToPlayer.x - offset.x, vectorToPlayer.y, 0));
            offset.x += 0.02f;
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
        if (GameObservables.gamePaused)
        {
            return;
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
        Vector2 movement = vectorToPlayer.normalized * stats.GetActualMovespeed() * Time.fixedDeltaTime;
        if (cat) movement *= -1;
        GetComponent<Rigidbody2D>().velocity = movement;
    }
    
    IEnumerator MakeInvulnerable(float time)
    {
        if (GameObservables.gamePaused)
        {
            yield return null;
        }

        invulnerable = true;
        yield return new WaitForSeconds(time);
        invulnerable = false;
    }

    public void OnHit(float damage)
    {
        if (invulnerable)
        {
            return;
        }
        //todo: damage reduction

        GetComponent<PlayerStats>().currentHealth -= damage;
        state = State.Defending;

        StartCoroutine(MakeInvulnerable(0.8f));

        Debug.Log("Monster HP = " + stats.currentHealth);

        if (stats.currentHealth <= 0) //todo: death animation or particles
        {
            ItemSpawner.instance.Spawn(GetComponent<LootTable>(), transform.position);
            Destroy(gameObject, 0.2f);
        }
    }
}
