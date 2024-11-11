using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGemBoss : MonoBehaviour
{
    private bool dying = false;
    public int HP = 500;
    private bool canFire = false;
    private Transform player = null;

    public Projectile bullet;
    public GameObject hpBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    public IEnumerator BattleBegin()
    {
        try
        {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            this.transform.position = new Vector2(-2.47f, 7.1f);
        }
        catch { }


        for (int i = 1; i <= 30; i++)
        {
            try
            {
                this.transform.position += new Vector3(0, -0.15f, 0);
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

    // Update is called once per frame
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
                    StartCoroutine(AttackIce());

                if (attack == 2)
                    StartCoroutine(AttackWater());

            }
        }
    }

    IEnumerator AttackIce()
    {
        canFire = false;
        int pNr = 0;



        if (GameObject.FindGameObjectsWithTag("Player") != null)
            pNr = GameObject.FindGameObjectsWithTag("Player").Length;

        player = null;
        if (pNr != 0)
            player = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, pNr)].transform;


        for (int ii = 1; ii <= 4; ii++)
        {
            int bulletNumber = 0;

            if (ii % 2 == 0)
                bulletNumber = 8;
            else
                bulletNumber = 9;
            if (!dying)
            {
                for (int count = 1; count <= bulletNumber; count++)
                {

                    try
                    {
                        Projectile proj = Instantiate(bullet, transform.position,
                            Quaternion.identity);

                        proj.gameObject.GetComponent<CircleCollider2D>().isTrigger = true;

                        float degreeModifier = 20f;

                        if (count == bulletNumber && ((bulletNumber % 2) == 1))
                        {

                            degreeModifier = 0;
                        }
                        else
                        {
                            if (count % 2 == 0)
                            {
                                degreeModifier = degreeModifier * (count / 2);

                                if (bulletNumber % 2 == 0)
                                    degreeModifier -= 10f;
                            }
                            else
                            {
                                degreeModifier = -1 * degreeModifier * (count + 1) / 2;
                                if (bulletNumber % 2 == 0)
                                    degreeModifier += 10f;
                            }
                        }

                        Vector2 position = new Vector2(Mathf.Cos((270 + degreeModifier) * Mathf.Deg2Rad),
                            Mathf.Sin((270 + degreeModifier) * Mathf.Deg2Rad))
                            * 6f;

                        proj.targetX = transform.position.x + position.x;
                        proj.targetY = transform.position.y + position.y;


                        proj.speed = 4f;
                        proj.lifeTime = 8f;
                    }
                    catch { }



                }


            }

            yield return new WaitForSeconds(0.7f);

        }




        float time2 = Random.Range(30, 50) / 10f;
        yield return new WaitForSeconds(time2);

        canFire = true;

    }

    IEnumerator AttackWater()
    {
        canFire = false;
        int pNr = 0;



        if (GameObject.FindGameObjectsWithTag("Player") != null)
            pNr = GameObject.FindGameObjectsWithTag("Player").Length;

        player = null;
        if (pNr != 0)
            player = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, pNr)].transform;






        float time2 = Random.Range(25, 45) / 10f;
        yield return new WaitForSeconds(time2);

        canFire = true;

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
