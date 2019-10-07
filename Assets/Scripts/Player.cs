using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Facing
    {
        Up,
        Down,
        Left,
        Right
    }

    public struct FrameData
    {
        public Vector2 moveDirection;
        public bool interacted;
        public bool attack;
        public Facing facing;
    };

    public FrameData lastFrame;
    public FrameData thisFrame;

    PlayerStats stats;

    bool attacking = false;
    public bool dead = false;
    bool invulnerable = false;
    public bool meleeWeapon = true;
    string itemName = "";

    public void OnWeaponEquip(Item item)
    {
        if (item.type == ItemType.Weapon)
        {
            itemName = item.name;
            meleeWeapon = true;
            for (int i = 0; i < item.traits.Length; ++i)
            {
                if (item.traits[i] == "Ranged")
                {
                    meleeWeapon = false;
                }
            }

            if (transform.childCount > 3) transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite = ItemDatabase.instance.getSprite(item.spriteIdx);
        }
    }

    public void OnWeaponUnequip()
    {
        meleeWeapon = true;
        if (transform.childCount > 3) transform.GetChild(3).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }

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
        if (GameObservables.gamePaused)
        {
            return;
        }

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
        if (attacking) return;
        Vector2 movement = thisFrame.moveDirection * stats.GetActualMovespeed() * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    void OnDeath()
    {
        if (!dead)
        {
            MessageQueue.addToQueue("After being bullied by various forest monsters you decided to leave all your items behind and go home to teach them a lesson");
            dead = true;
            PlayerInventory.instance.removeAllItemsAndEquipment();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Shopping");
        }
    }

    void Animate()
    {
        if (attacking) return;
        if (thisFrame.moveDirection.x == -1)
        {
            thisFrame.facing = Facing.Left;
            if (thisFrame.facing != lastFrame.facing)
            {
                GetComponentInChildren<AnimationStateController>().SetState("WalkLeft");
            }
        }
        else if (thisFrame.moveDirection.x == 1)
        {
            thisFrame.facing = Facing.Right;
            if (thisFrame.facing != lastFrame.facing)
            {
                GetComponentInChildren<AnimationStateController>().SetState("WalkRight");
            }
        }
        else if (thisFrame.moveDirection.y == -1)
        {
            thisFrame.facing = Facing.Down;
            if (thisFrame.facing != lastFrame.facing)
            {
                GetComponentInChildren<AnimationStateController>().SetState("WalkDown");
            }
        }
        else if (thisFrame.moveDirection.y == 1)
        {
            thisFrame.facing = Facing.Up;
            if (thisFrame.facing != lastFrame.facing)
            {
                GetComponentInChildren<AnimationStateController>().SetState("WalkUp");
            }
        }

        if (thisFrame.moveDirection == Vector2.zero)
        {
            if (lastFrame.moveDirection.x == -1)
            {
                thisFrame.facing = Facing.Left;
                GetComponentInChildren<AnimationStateController>().SetState("IdleLeft");
            }
            else if (lastFrame.moveDirection.x == 1)
            {
                thisFrame.facing = Facing.Right;
                GetComponentInChildren<AnimationStateController>().SetState("IdleRight");
            }
            else if (lastFrame.moveDirection.y == -1)
            {
                thisFrame.facing = Facing.Down;
                GetComponentInChildren<AnimationStateController>().SetState("IdleDown");
            }
            else if (lastFrame.moveDirection.y == 1)
            {
                thisFrame.facing = Facing.Up;
                GetComponentInChildren<AnimationStateController>().SetState("IdleUp");
            }
        }
    }

    void Update()
    {
        if (GameObservables.gamePaused)
        {
            return;
        }

        if (stats.currentHealth <= 0)
        {
            if (!dead)
            {
                OnDeath();
            }
            return;
        }

        HandleInput();
        Animate();
        Attack();
        if (transform.childCount > 3)
            if (thisFrame.facing == Facing.Up)
            {
                transform.GetChild(3).gameObject.GetComponentInChildren<SpriteOrderYSort>().orderOffset = -10;
                Vector3 scale = transform.GetChild(3).gameObject.transform.localScale;
                scale.x = -1;
                scale.y = 1;
                transform.GetChild(3).gameObject.transform.localScale = scale;
            }
            else if (thisFrame.facing == Facing.Down)
            {
                transform.GetChild(3).gameObject.GetComponentInChildren<SpriteOrderYSort>().orderOffset = 10;
                Vector3 scale = transform.GetChild(3).gameObject.transform.localScale;
                scale.x = 1;
                scale.y = 1; //wanna make it -1 but then hand position is wrong which is effort to recalc everywhere
                transform.GetChild(3).gameObject.transform.localScale = scale;
            }
            else if (thisFrame.facing == Facing.Right)
            {
                transform.GetChild(3).gameObject.GetComponentInChildren<SpriteOrderYSort>().orderOffset = -10;
                Vector3 scale = transform.GetChild(3).gameObject.transform.localScale;
                scale.x = -1;
                scale.y = 1;
                transform.GetChild(3).gameObject.transform.localScale = scale;
            }
            else
            {
                transform.GetChild(3).gameObject.GetComponentInChildren<SpriteOrderYSort>().orderOffset = -10;
                Vector3 scale = transform.GetChild(3).gameObject.transform.localScale;
                scale.x = 1;
                scale.y = 1;
                transform.GetChild(3).gameObject.transform.localScale = scale;
            }
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
        GetComponent<Rigidbody2D>().mass = 10;
        SFXManager.instance.PlaySFX("Break");

        if (meleeWeapon)
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }

        if (transform.childCount > 3) transform.GetChild(3).gameObject.SetActive(true);
        attacking = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Vector2 facingDir = Vector2.zero;
        if (thisFrame.facing == Facing.Left)
        {
            facingDir.x = -1;
        }
        else if (thisFrame.facing == Facing.Right)
        {
            facingDir.x = 1;
        }
        else if (thisFrame.facing == Facing.Up)
        {
            facingDir.y = 1;
        }
        else if (thisFrame.facing == Facing.Down)
        {
            facingDir.y = -1;
        }

        if (meleeWeapon)
        {
            transform.GetChild(0).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
            transform.GetChild(1).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
            float range = stats.GetActualWeaponRange() / 100.0f; //Pixels Per Unit
            transform.GetChild(2).transform.Translate(new Vector3(facingDir.x * range, facingDir.y * range, 0), Space.Self);
            if (transform.GetChild(2).childCount > 0) transform.GetChild(2).GetChild(0).transform.Translate(new Vector3(facingDir.x * range - (facingDir.x * 0.085f), facingDir.y * range - (facingDir.y * 0.085f), 0), Space.Self);
            if (transform.childCount > 3) transform.GetChild(3).transform.Translate(new Vector3(facingDir.x * 0.1f, facingDir.y * 0.1f, 0), Space.Self);
        }
        else
        {
            ProjectileSpawner.instance.SpawnProjectile((Vector2)transform.position + facingDir * 0.2f, itemName, facingDir, stats.GetActualDamage(), 300, stats.GetActualWeaponRange() * 10, true);
        }

        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).transform.localPosition = Vector3.zero;
        transform.GetChild(1).transform.localPosition = Vector3.zero;
        transform.GetChild(2).transform.localPosition = Vector3.zero;
        if (transform.GetChild(2).childCount > 0) transform.GetChild(2).GetChild(0).transform.localPosition = Vector3.zero;
        if (transform.childCount > 3) transform.GetChild(3).transform.localPosition = Vector3.zero;
        transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        if (transform.childCount > 3) transform.GetChild(3).gameObject.SetActive(false);
        attacking = false;
        GetComponent<Rigidbody2D>().mass = 0.1f;
        yield return null;
    }

    private void FixedUpdate()
    {
        if (GameObservables.gamePaused)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            return;
        }

        if (stats.currentHealth <= 0)
        {
            if (!dead)
            {
                OnDeath();
            }
            return;
        }

        if (!attacking)
        {
            Move();
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.transform.parent)
        {
            if (collider.transform.tag == "Enemy") //todo: tag/layer
            {
                Vector2 direction = (collider.transform.position - transform.position).normalized;
                collider.transform.parent.GetComponent<Monster>().OnHit(stats.GetActualDamage(), direction * 0.3f);
            }
        }
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

        damage -= stats.GetActualArmour(); //yay heals
        damage = Mathf.Max(damage, 1); //ricardo is lazy

        GetComponent<PlayerStats>().currentHealth -= damage;
        StartCoroutine(MakeInvulnerable(0.8f));

        if (stats.currentHealth <= 0 && !dead) //todo: death animation or particles
        {
            OnDeath();
        }
    }
}