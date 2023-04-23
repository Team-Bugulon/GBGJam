using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Jelly : MonoBehaviour
{
    [SerializeField] float stunDuration = 3f;
    [SerializeField] float jumpPeriod = 2f;
    [SerializeField] float jumpStrength;

    float jumpTimer;
    int direction = 1;
    StunEnemies stunEnemies;

    private void Start()
    {
        stunEnemies = GetComponent<StunEnemies>();
    }

    private void FixedUpdate()
    {
        if (!stunEnemies.stunned) jumpTimer += Time.deltaTime;
        if (jumpTimer >= jumpPeriod)
        {
            jumpTimer = 0;
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpStrength, ForceMode2D.Impulse);
            GetComponent<Animator>().Play("jelly_move");
        }
    }

    public void onSnap()
    {
       
        if (!stunEnemies.stunned)
        {
            stunEnemies.stunned = true;
            GetComponent<Animator>().Play("jelly_stun");
            transform.DOComplete();
            transform.DOShakeScale(.6f, .5f, 20);
            Invoke("Unstun", stunDuration);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CircleCollider2D>().enabled = false;
            SoundManager.i.Play("Stun2", .2f, .8f);
        }
    }

    void Unstun()
    {
        stunEnemies.stunned = false;
        transform.DOComplete();
        transform.DOShakeScale(.4f, .5f, 20);
        GetComponent<Animator>().Play("jelly_idle");
        GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
