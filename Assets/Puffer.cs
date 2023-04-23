using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puffer : MonoBehaviour
{
    [SerializeField] float stunDuration = 3f;
    [SerializeField] float speed;

    int direction = 1;

    private void Start()
    {
        if (direction < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);
    }

    //private void FixedUpdate()
    //{
    //    GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction *= -1;
        if (direction < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);
        } else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);
        }
    }


    public void onSnap()
    {
        var stunEnemies = GetComponent<StunEnemies>();
        if (!stunEnemies.stunned)
        {
            stunEnemies.stunned = true;
            GetComponent<Animator>().Play("puffer_stun");
            transform.DOComplete();
            transform.DOShakeScale(.6f, .5f, 20);
            Invoke("Unstun", stunDuration);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = false;
            SoundManager.i.Play("Stun1", .2f, .8f);
        }
    }

    void Unstun()
    {
        GetComponent<StunEnemies>().stunned = false;
        transform.DOComplete();
        transform.DOShakeScale(.4f, .5f, 20);
        GetComponent<Animator>().Play("puffer_idle");
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);


    }
}
