using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthGemBoss : MonoBehaviour
{
    public int HP = 300;
    private bool dying = false;
    private bool canFire = false;
    private Transform player = null;

    public Projectile bullet;

    public GameObject hpBar;

    void Start()
    {
    }

    public IEnumerator BattleBegin()
    {
        try
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.transform.position = new Vector2(11, 5);
        }
        catch { }


        for (int i = 1; i <= 30; i++)
        {
            try
            {
                this.transform.position += new Vector3(-0.14f, -0.07f, 0);
            }
            catch { }


            yield return new WaitForSeconds(0.07f);
        }

        try
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
        catch { }

        yield return new WaitForSeconds(1f);

        try
        {
            canFire = true;
        }
        catch { }

    }

    void Update()
    {
        float perc = HP / 300f;
        if (perc < 0)
            perc = 0;
        hpBar.transform.localScale = new Vector2(2.18f * (float)(perc), hpBar.transform.localScale.y);

        if (!dying)
        {

            if (canFire == true)
            {
                int attack = Random.Range(1, 2);

                if (attack == 1)
                    StartCoroutine(AttackSimple());


            }
        }
    }

    IEnumerator AttackSimple()
    {
        canFire = false;
        int pNr = 0;



        if (GameObject.FindGameObjectsWithTag("Player") != null)
            pNr = GameObject.FindGameObjectsWithTag("Player").Length;

        player = null;
        if (pNr != 0)
            player = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, pNr)].transform;



        if (GameObject.FindGameObjectsWithTag("Player") != null)
            pNr = GameObject.FindGameObjectsWithTag("Player").Length;

        player = null;
        if (pNr != 0)
            player = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, pNr)].transform;

        if (player != null && !dying)
        {
            try
            {
                float targX = player.position.x;
                float targY = player.position.y;


                Projectile projectile =
                    Instantiate(bullet, transform.position,
                Quaternion.identity);

                projectile.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;


                projectile.targetX = targX;
                projectile.targetY = targY;
                projectile.speed = 4f;
                projectile.lifeTime = 6f;



            }
            catch { }




        }



        float time = Random.Range(50, 90) / 10f;
        yield return new WaitForSeconds(time);


        canFire = true;
        //StartCoroutine(Shooting());
    }


    void TakeDamage(int d)
    {
        HP -= d;

        if (HP <= 0)
        {
            if (!dying)
            {
                dying = true;
                StartCoroutine(Dying());
            }
            // Destroy(this.gameObject);

        }

    }

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 1; i <= 18; i++)
        {
            this.transform.localScale = this.transform.localScale - new Vector3(0.05f, 0.05f, 0f);

            yield return new WaitForSeconds(0.06f);
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag.Equals("Bullet"))
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().dmg);

        }

    }


    IEnumerator Shooting()
    {
        canFire = false;
        yield return new WaitForSeconds(1f);
        canFire = true;
    }
}
