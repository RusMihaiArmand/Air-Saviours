using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int playerNumber = 1;

    public float moveSpeed = 3f;
    public int HP = 3, maxHP = 3;
    public Rigidbody2D rb;
    public Animator animator;
    string currentAnimation = "", animationToPlay;
    public bool attacking = false, attacked = false;
    private bool attackingSP = false, attackedSP = false;

    private bool invincible = false;


    public int bulletNumber = 1;
    public float bulletSeparation = 20f, bulletSpeed = 6.4f;
    public int bulletDmg = 1;

    public float rocketSpeed = 15f;
    public int rocketDmg = 10;

    public GameObject prop;

    public bool locked;

    public Projectile bullet;

    public int rocketNumber = 5;
    public Projectile rocket;

    public Vector2 movement;

    public AudioSource src;
    public AudioClip sfxFly;

    public float fireSpeed = 100;
    public int fireSpeedPowerups = 0;
    int dmgMultiplier = 1;
    int dmgPowerups = 0;

    public GameObject power_dmg;
    public GameObject power_quick;
    public GameObject power_shield;

    public void Reset()
    {
        this.StopAllCoroutines();
        movement.x = 0;
        movement.y = 0;
        HP = 3;
        rocketNumber = 5;
        fireSpeed = 100f; //fireSpeedPowerups = 0;
        dmgMultiplier = 1; // dmgPowerups = 0;
        invincible = false;

        power_dmg.SetActive(false);
        power_quick.SetActive(false);
        power_shield.SetActive(false);
    }

    public void SetPlayerNumber(int x)
    {
        playerNumber = x;
    }

    public int GetHP()
    {
        return HP;
    }

    void Start()
    {
        src.volume = .6f;
        src.clip = sfxFly;
        src.Play();
        locked = true;
    }

    public void Lock()
    {
        this.locked = true;
    }
    public void Unlock()
    {
        this.locked = false;
        attacking = false;
        attacked = false;
        attackingSP = false;
        attackedSP = false;

        System.Console.WriteLine("UNLOCK");
    }

    void FixedUpdate()
    {


        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        prop.transform.eulerAngles = new Vector3(prop.transform.eulerAngles.x, prop.transform.eulerAngles.y,
            prop.transform.eulerAngles.z - 5f);

        if (prop.transform.eulerAngles.z < -360)
            prop.transform.eulerAngles = new Vector3(prop.transform.eulerAngles.x, prop.transform.eulerAngles.y,
            prop.transform.eulerAngles.z + 360f);

    }

    // Update is called once per frame
    void Update()
    {


        if (!locked)
        {

            //movement.x = Input.GetAxisRaw("Horizontal");
            // movement.y = Input.GetAxisRaw("Vertical");

            if (GameStateManager._instance.numberOfPlayers == 1)
            {
                movement.x = Input.GetAxisRaw("Horizontal P1");
                if (movement.x == 0)
                    movement.x = Input.GetAxisRaw("Horizontal P2");

                movement.y = Input.GetAxisRaw("Vertical P1");
                if (movement.y == 0)
                    movement.y = Input.GetAxisRaw("Vertical P2");


            }
            else
            {
                if (playerNumber == 1)
                {
                    movement.x = Input.GetAxisRaw("Horizontal P1");
                    movement.y = Input.GetAxisRaw("Vertical P1");
                }
                else
                {
                    movement.x = Input.GetAxisRaw("Horizontal P2");
                    movement.y = Input.GetAxisRaw("Vertical P2");
                }

            }


            if (movement.x < 0)
                animationToPlay = "Left";
            if (movement.x == 0)
                animationToPlay = "Normal";
            if (movement.x > 0)
                animationToPlay = "Right";

            if (!currentAnimation.Equals(animationToPlay))
            {
                currentAnimation = animationToPlay;
                animator.Play(animationToPlay);
            }



            if (GameStateManager._instance.numberOfPlayers == 1)
            {
                if (Input.GetButtonDown("Fire P1") || Input.GetButtonDown("Fire P2"))
                {

                    attacking = true;
                }
                if (Input.GetButtonUp("Fire P1") || Input.GetButtonUp("Fire P2"))
                {
                    attacking = false;
                }



            }
            else
            {
                if (playerNumber == 1)
                {

                    if (Input.GetButtonDown("Fire P1"))
                    {

                        attacking = true;
                    }
                    if (Input.GetButtonUp("Fire P1"))
                    {
                        attacking = false;
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire P2"))
                    {

                        attacking = true;
                    }
                    if (Input.GetButtonUp("Fire P2"))
                    {
                        attacking = false;
                    }

                }

            }



            if (attacking && !attacked)
            {
                Projectile[] rangedProjectile = new Projectile[2];


                for (int count = 0; count <= 1; count++)
                {

                    rangedProjectile[count] = Instantiate(bullet,
                    new Vector2(transform.position.x - 0.1f + count * 0.2f, transform.position.y), Quaternion.identity);

                    rangedProjectile[count].gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                    rangedProjectile[count].gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0083f, 0.003f);



                    rangedProjectile[count].targetX = transform.position.x - 0.1f + count * 0.2f;
                    rangedProjectile[count].targetY = transform.position.y + 20f;
                    rangedProjectile[count].speed = bulletSpeed;
                    rangedProjectile[count].lifeTime = 5f;
                    rangedProjectile[count].dmg = bulletDmg * dmgMultiplier;


                    // rangedProjectile[count].gameObject.GetComponent<SpriteRenderer>().sprite = bulletSprite;

                }





                StartCoroutine(AttackCooldown());
            }



            if (GameStateManager._instance.numberOfPlayers == 1)
            {
                if (Input.GetButtonDown("Fire Special P1") || Input.GetButtonDown("Fire Special P2"))
                {

                    attackingSP = true;
                }
                if (Input.GetButtonUp("Fire Special P1") || Input.GetButtonUp("Fire Special P2"))
                {
                    attackingSP = false;
                }



            }
            else
            {
                if (playerNumber == 1)
                {

                    if (Input.GetButtonDown("Fire Special P1"))
                    {

                        attackingSP = true;
                    }
                    if (Input.GetButtonUp("Fire Special P1"))
                    {
                        attackingSP = false;
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire Special P2"))
                    {

                        attackingSP = true;
                    }
                    if (Input.GetButtonUp("Fire Special P2"))
                    {
                        attackingSP = false;
                    }

                }

            }




            if (attackingSP && !attackedSP && rocketNumber > 0)
            {

                rocketNumber--;
                Projectile rangedProjectile;

                rangedProjectile = Instantiate(rocket,
                    new Vector2(transform.position.x, transform.position.y + 0.1f), Quaternion.identity);

                rangedProjectile.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                rangedProjectile.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0083f, 0.003f);


                rangedProjectile.targetX = transform.position.x;
                rangedProjectile.targetY = transform.position.y + 20f;
                rangedProjectile.speed = rocketSpeed;
                rangedProjectile.lifeTime = 5f;
                rangedProjectile.dmg = rocketDmg * dmgMultiplier;

                StartCoroutine(AttackCooldownSP());
            }


        }
    }

    IEnumerator FireSpeedTimer()
    {
        // yield return new WaitForSeconds(15f);

        yield return new WaitForSeconds(5f);

        power_quick.GetComponent<SpriteRenderer>().color = new Color(
            power_quick.GetComponent<SpriteRenderer>().color.r,
            power_quick.GetComponent<SpriteRenderer>().color.g,
            power_quick.GetComponent<SpriteRenderer>().color.b,
            0.66f);



        yield return new WaitForSeconds(5f);

        power_quick.GetComponent<SpriteRenderer>().color = new Color(
            power_quick.GetComponent<SpriteRenderer>().color.r,
            power_quick.GetComponent<SpriteRenderer>().color.g,
            power_quick.GetComponent<SpriteRenderer>().color.b,
            0.33f);

        yield return new WaitForSeconds(5f);

        power_quick.GetComponent<SpriteRenderer>().color = new Color(
            power_quick.GetComponent<SpriteRenderer>().color.r,
            power_quick.GetComponent<SpriteRenderer>().color.g,
            power_quick.GetComponent<SpriteRenderer>().color.b,
            0.01f);


        fireSpeed = 100;
        power_quick.SetActive(false);

        yield return new WaitForSeconds(0f);
    }

    IEnumerator DoubleDmgTimer()
    {
        //yield return new WaitForSeconds(30f);


        yield return new WaitForSeconds(10f);

        power_dmg.GetComponent<SpriteRenderer>().color = new Color(
            power_dmg.GetComponent<SpriteRenderer>().color.r,
            power_dmg.GetComponent<SpriteRenderer>().color.g,
            power_dmg.GetComponent<SpriteRenderer>().color.b,
            0.66f);



        yield return new WaitForSeconds(10f);

        power_dmg.GetComponent<SpriteRenderer>().color = new Color(
            power_dmg.GetComponent<SpriteRenderer>().color.r,
            power_dmg.GetComponent<SpriteRenderer>().color.g,
            power_dmg.GetComponent<SpriteRenderer>().color.b,
            0.33f);

        yield return new WaitForSeconds(10f);

        power_dmg.GetComponent<SpriteRenderer>().color = new Color(
            power_dmg.GetComponent<SpriteRenderer>().color.r,
            power_dmg.GetComponent<SpriteRenderer>().color.g,
            power_dmg.GetComponent<SpriteRenderer>().color.b,
            0.01f);


        dmgMultiplier = 1;
        power_dmg.SetActive(false);


        yield return new WaitForSeconds(0f);
    }


    IEnumerator AttackCooldown()
    {
        attacked = true;

        yield return new WaitForSeconds(0.2f * 100f / fireSpeed);

        attacked = false;

    }

    IEnumerator AttackCooldownSP()
    {
        attackedSP = true;

        yield return new WaitForSeconds(3f * 100f / fireSpeed);

        attackedSP = false;

    }


    void TakeDamage(int d)
    {
        if (!invincible)
        {
            StartCoroutine(Invincibility());
            HP -= d;
            if (HP < 0)
                HP = 0;

            if (HP <= 0)
            {
                //SOMETHING
                StartCoroutine(Fail());


            }
        }
    }

    IEnumerator Fail()
    {
        locked = true;
        movement.x = 0;
        movement.y = 0;
        yield return new WaitForSeconds(1f);

        StopCoroutine(FireSpeedTimer());
        fireSpeedPowerups = 0;
        fireSpeed = 100;
        power_quick.SetActive(false);

        StopCoroutine(DoubleDmgTimer());
        dmgPowerups = 0;
        dmgMultiplier = 1;
        power_dmg.SetActive(false);

        //GameStateManager._instance.walls.SetActive(false);

        this.transform.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        for (int i = 0; i <= 60; i++)
        {
            transform.position = new Vector2(transform.position.x,
                  transform.position.y - 0.1f);
            yield return new WaitForSeconds(0.03f);
        }

        this.transform.gameObject.GetComponent<BoxCollider2D>().enabled = true;

        this.gameObject.SetActive(false);
        // GameStateManager._instance.BackToMenu();
    }

    private IEnumerator Invincibility()
    {
        invincible = true;

        yield return new WaitForSeconds(2f);

        invincible = false;

    }

    public void ReviveInvincibility1()
    {
        invincible = true;
        power_shield.SetActive(true);
        power_shield.GetComponent<SpriteRenderer>().color = new Color(
            power_shield.GetComponent<SpriteRenderer>().color.r,
            power_shield.GetComponent<SpriteRenderer>().color.g,
            power_shield.GetComponent<SpriteRenderer>().color.b,
            1);
    }

    public IEnumerator ReviveInvincibility2()
    {
        invincible = true;

        yield return new WaitForSeconds(0.5f);

        power_shield.GetComponent<SpriteRenderer>().color = new Color(
            power_shield.GetComponent<SpriteRenderer>().color.r,
            power_shield.GetComponent<SpriteRenderer>().color.g,
            power_shield.GetComponent<SpriteRenderer>().color.b,
            0.5f);


        yield return new WaitForSeconds(0.25f);

        power_shield.GetComponent<SpriteRenderer>().color = new Color(
            power_shield.GetComponent<SpriteRenderer>().color.r,
            power_shield.GetComponent<SpriteRenderer>().color.g,
            power_shield.GetComponent<SpriteRenderer>().color.b,
            0.25f);

        yield return new WaitForSeconds(0.25f);

        power_shield.GetComponent<SpriteRenderer>().color = new Color(
            power_shield.GetComponent<SpriteRenderer>().color.r,
            power_shield.GetComponent<SpriteRenderer>().color.g,
            power_shield.GetComponent<SpriteRenderer>().color.b,
            0f);


        invincible = false;
        power_shield.SetActive(false);

    }

    private void Effect(int ef)
    {
        if (ef == 1)
        {
            //fire rate

            StopCoroutine(FireSpeedTimer());

            fireSpeed = 200;
            //fireSpeedPowerups++;

            power_quick.GetComponent<SpriteRenderer>().color = new Color(
            power_quick.GetComponent<SpriteRenderer>().color.r,
            power_quick.GetComponent<SpriteRenderer>().color.g,
            power_quick.GetComponent<SpriteRenderer>().color.b,
            1);

            power_quick.SetActive(true);
            StartCoroutine(FireSpeedTimer());
        }

        if (ef == 2)
        {
            //x2 dmg
            /*
            dmgMultiplier = 2;
            dmgPowerups++;
            power_dmg.SetActive(true);
            StartCoroutine(DoubleDmgTimer());
            */

            StopCoroutine(DoubleDmgTimer());

            dmgMultiplier = 2;

            power_dmg.GetComponent<SpriteRenderer>().color = new Color(
            power_dmg.GetComponent<SpriteRenderer>().color.r,
            power_dmg.GetComponent<SpriteRenderer>().color.g,
            power_dmg.GetComponent<SpriteRenderer>().color.b,
            1);

            power_dmg.SetActive(true);
            StartCoroutine(DoubleDmgTimer());
        }


        if (ef == 3)
        {
            //rocket
            rocketNumber += 3;
        }

        if (ef == 4)
        {
            //life

            if (HP < 3)
                HP++;

        }

        if (ef == 5)
        {
            //REVIVE

        }



    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            TakeDamage(1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        //if (collision.gameObject.tag.Equals("Enemy"))
        //{
        //    TakeDamage(1);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("EnemyBullet"))
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().dmg);
        }

        if (collision.tag.Equals("Enemy"))
        {
            TakeDamage(1);
        }


        if (collision.tag.Equals("PowerUp"))
        {
            if (!locked)
                Effect(collision.gameObject.GetComponent<PowerUp>().eff);
        }

    }
}
