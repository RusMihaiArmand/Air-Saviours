using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int eff;

    void Start()
    {
        StartCoroutine(Decay());
    }
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y - 40f), 1f * Time.deltaTime);
    }

    IEnumerator Decay()
    {
        yield return new WaitForSeconds(20f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if (eff == 5)
            {
                GameStateManager._instance.Revive();

            }

            Destroy(this.gameObject);
        }

    }

}
