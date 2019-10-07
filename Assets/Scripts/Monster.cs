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

    [SerializeField] State oldState = State.Idle;
    [SerializeField] State state = State.Idle;

    Vector2 vectorToPlayer;
    float distanceToPlayer = 9999;
    [SerializeField] bool playerInLineOfSight = false;
    public bool cat = false;
    public bool aggresive = true;

    PlayerStats stats;
    bool invulnerable = false;
    bool willAttackPlayer = false;
    bool doneAttacking = true;

    float attackDelayTimer = 0;
    Player.Facing oldFacing = Player.Facing.Down;
    Player.Facing facing = Player.Facing.Down;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = GetComponent<PlayerStats>();
        GetComponentInChildren<AnimationStateController>().SetState("Idle");
    }

    void CheckState()
    {
        if (state == State.Idle)
        {
            if (state != oldState)
            {
                GetComponentInChildren<AnimationStateController>().SetState("Idle");
            }

            if (willAttackPlayer &&
                distanceToPlayer < 1 &&
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
                GetComponentInChildren<AnimationStateController>().SetState("Idle");
                state = State.Idle;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }

            if (distanceToPlayer < 0.30 &&
                playerInLineOfSight &&
                !cat)
            {
                AnimateFromFacing();
                state = State.Attacking;
                SetAttackAnimationFromFacing();
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        else if (state == State.Attacking)
        {
            if (doneAttacking)
            {
                if (transform.childCount > 3 &&
                    attackDelayTimer > 0.5f)
                {
                    StartCoroutine(DoAttack());
                }
                else if (distanceToPlayer > 0.30)
                {
                    AnimateFromFacing();
                    state = State.Hunting;
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }
        }
        else if (state == State.Defending)
        {
            if (doneAttacking)
                StartCoroutine(Defending());
        }
    }

    IEnumerator DoAttack()
    {
        Debug.Log("yo");
        doneAttacking = false;
        transform.GetChild(3).gameObject.SetActive(true);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Vector2 facingDir = vectorToPlayer.normalized;

        transform.GetChild(0).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
        transform.GetChild(1).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
        float range = stats.GetActualWeaponRange() / 100.0f; //Pixels Per Unit
        transform.GetChild(3).transform.Translate(new Vector3(facingDir.x * range, facingDir.y * range, 0), Space.Self);

        yield return new WaitForSeconds(0.2f);

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(1).transform.localPosition = Vector3.zero;
        transform.GetChild(3).transform.localPosition = Vector3.zero;

        yield return new WaitForSeconds(0.1f);

        transform.GetChild(3).gameObject.SetActive(false);

        doneAttacking = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        attackDelayTimer = 0.0f;
        yield return null;
    }

    IEnumerator Defending()
    {
        if (cat) yield return null;
        SetDefendAnimationFromFacing();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        state = State.Attacking;
        if (distanceToPlayer > 0.75)
            StartCoroutine(SpeedUpAfterDefending());
        SetAttackAnimationFromFacing();
    }

    IEnumerator SpeedUpAfterDefending()
    {
        Vector2 movement = vectorToPlayer.normalized * stats.GetActualMovespeed() * Time.fixedDeltaTime;
        if (cat) movement *= -1.5f;
        GetComponent<Rigidbody2D>().velocity = movement;
        state = State.Hunting;
        stats.moveSpeedModifier += 6;
        yield return new WaitForSeconds(0.4f);
        movement = vectorToPlayer.normalized * stats.GetActualMovespeed() * Time.fixedDeltaTime;
        if (cat) movement *= -1.5f;
        GetComponent<Rigidbody2D>().velocity = movement;
        state = State.Hunting;
        stats.moveSpeedModifier -= 6;
        yield return null;
    }

    private void Update()
    {
        if (GameObservables.gamePaused)
        {
            return;
        }

        if (!doneAttacking) return;

        attackDelayTimer += Time.deltaTime;

        willAttackPlayer = aggresive || stats.currentHealth < stats.GetActualMaxHealth();
        if (cat) willAttackPlayer = true;

        vectorToPlayer = player.transform.position - transform.position;
        distanceToPlayer = Mathf.Abs(vectorToPlayer.magnitude);

        Vector2 facingDir = vectorToPlayer.normalized;

        oldFacing = facing;

        bool horizontal = Mathf.Abs(facingDir.x) > Mathf.Abs(facingDir.y);
        if (horizontal)
        {
            facing = facingDir.x < 0 ? Player.Facing.Left : Player.Facing.Right;
        }
        else
        {
            facing = facingDir.y < 0 ? Player.Facing.Down : Player.Facing.Up;
        }

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

        oldState = state;
        CheckState();
        AnimateFromFacing();

        switch (state)
        {
            case State.Idle: IdleUpdate(); break;
            case State.Patrol: PatrolUpdate(); break;
            case State.Death: DeathUpdate(); break;
            case State.Attacking: AttackingUpdate(); break;
            case State.Defending: DefendingUpdate(); break;
            case State.Hunting: HuntingUpdate(); break;
        }
    }

    private void AnimateFromFacing()
    {
        if (state != State.Idle)
        {
            if (facing != oldFacing)
            {
                if (facing == Player.Facing.Left)
                {
                    GetComponentInChildren<AnimationStateController>().SetState("WalkLeft");
                }
                else if (facing == Player.Facing.Right)
                {
                    GetComponentInChildren<AnimationStateController>().SetState("WalkRight");
                }
                else if (facing == Player.Facing.Up)
                {
                    GetComponentInChildren<AnimationStateController>().SetState("WalkUp");
                }
                else if (facing == Player.Facing.Down)
                {
                    GetComponentInChildren<AnimationStateController>().SetState("WalkDown");
                }
            }
        }
    }

    private void SetAttackAnimationFromFacing()
    {
        if (facing == Player.Facing.Left)
        {
            GetComponentInChildren<AnimationStateController>().SetState("AttackLeft");
        }
        else if (facing == Player.Facing.Right)
        {
            GetComponentInChildren<AnimationStateController>().SetState("AttackRight");
        }
        else if (facing == Player.Facing.Up)
        {
            GetComponentInChildren<AnimationStateController>().SetState("AttackUp");
        }
        else if (facing == Player.Facing.Down)
        {
            GetComponentInChildren<AnimationStateController>().SetState("AttackDown");
        }
    }
    private void SetDefendAnimationFromFacing()
    {
        if (facing == Player.Facing.Left)
        {
            GetComponentInChildren<AnimationStateController>().SetState("DefendLeft");
        }
        else if (facing == Player.Facing.Right)
        {
            GetComponentInChildren<AnimationStateController>().SetState("DefendRight");
        }
        else if (facing == Player.Facing.Up)
        {
            GetComponentInChildren<AnimationStateController>().SetState("DefendUp");
        }
        else if (facing == Player.Facing.Down)
        {
            GetComponentInChildren<AnimationStateController>().SetState("DefendDown");
        }
    }

    private void FixedUpdate()
    {
        if (!doneAttacking)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (GameObservables.gamePaused) return;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (!doneAttacking) return;

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
        if (cat) movement *= -1.5f;
        GetComponent<Rigidbody2D>().velocity = movement;
    }
    
    IEnumerator MakeInvulnerable(float time)
    {
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

        damage -= stats.GetActualArmour(); //monsters can heal :)
        damage = Mathf.Max(damage, 1); //thomas is mean

        GetComponent<PlayerStats>().currentHealth -= damage;
        state = State.Defending;

        StartCoroutine(MakeInvulnerable(0.8f));

        if (stats.currentHealth <= 0) //todo: death animation or particles
        {
            ItemSpawner.instance.Spawn(GetComponent<LootTable>(), transform.position);
            Destroy(gameObject, 0.2f);
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.parent &&
            collider.name == "Collider" &&
            collider.transform.parent.tag == "Player")
        {
            collider.transform.parent.GetComponent<Player>().OnHit(stats.GetActualDamage());
        }
    }

    //todo: deal damage to player on trigger
}
