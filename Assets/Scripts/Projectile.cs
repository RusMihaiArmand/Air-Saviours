using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float targetX = 0, targetY = 0;
    public float speed = 0, lifeTime = 1;
    public int dmg = 1;

    public AudioSource src;

    public static GameObject blast;
    private GameObject target;


    void Start()
    {
        if (src != null)
            src.Play();

        StartCoroutine(Decay());
        targetX = targetX + (targetX - transform.position.x) * 10;
        targetY = targetY + (targetY - transform.position.y) * 10;
    }

    public void SetTarget(GameObject go)
    {
        target = go;
        StartCoroutine(ResetTarget(go.transform.position.x, go.transform.position.y));


    }

    public IEnumerator ResetTarget(float tX, float tY)
    {
        float time;
        try
        {
            targetX = tX + (tX - transform.position.x) * 10;
            targetY = tY + (tY - transform.position.y) * 10;
            time = Random.Range(10, 21) / 10f;
        }
        catch
        {
            time = -1;
        }

        if (time > 0)
        {
            yield return new WaitForSeconds(time);

            if (target != null)
                if (target.active)
                {
                    SetTarget(target);
                }
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(targetX, targetY), speed * Time.deltaTime);
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(this.gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.tag.Equals("Wall"))
        {
            Destroy(this.gameObject);

        }


    }

    private IEnumerator Detonate()
    {

        speed = 0;
        this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

        Destroy(this.GetComponent<SpriteRenderer>());
        Destroy(this.GetComponent<Rigidbody2D>());
        Destroy(this.GetComponent<BoxCollider2D>());

        GameObject[] go = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject g in go)
        {
            Destroy(g);
        }


        yield return new WaitForSeconds(0.01f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((collision.tag.Equals("Player") || collision.tag.Equals("Blast")) && this.tag.Equals("EnemyBullet"))
            || (collision.tag.Equals("Wall") && this.tag.Equals("Bullet"))
            || (collision.tag.Equals("Enemy") && this.tag.Equals("Bullet"))
            || collision.tag.Equals("OuterWall"))
        {
            if (this.tag.Equals("Bullet") && this.name.Contains("Rocket"))
                StartCoroutine(Detonate());
            else
                Destroy(this.gameObject);

        }

    }

}
