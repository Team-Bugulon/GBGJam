using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Gameplay")]
    public float speed;
    public float speedMax;
    public float deceleration;
    public float collisionForce = 1000;
    public float stunTime = 1f;
    [Header("Managment")]
    public bool controlsLocked;
    public bool stunned = false;
    [Header("Items")]
    public GameObject spotLight;
    [Header("Sprites")]
    public List<Sprite> sprites;

    Vector2 movementInput;
    Vector2 actualSpeed;
    Rigidbody2D rb;
    BoxCollider2D cc;
    SpriteRenderer sr;



    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !controlsLocked)
        {
            movementInput = ctx.ReadValue<Vector2>();
        }
        else if (ctx.canceled)
        {
            movementInput = Vector2.zero;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        //make material unique
        sr.material = new Material(sr.material);
        actualSpeed = Vector2.zero;
    }

    private void Update()
    {
        if (!stunned)
        {
            var diff = (Vector2)(UIManager.i.cursorSprite.transform.position - transform.position);
            if (diff.y > 0)
            {
                sr.sprite = sprites[1];
            } else if (diff.y < 0)
            {
                sr.sprite = sprites[0];
            }

            if (diff.x > 0)
            {
                sr.flipX = false;
            } else if (diff.x < 0)
            {
                sr.flipX = true;
            }
        } else
        {
            sr.sprite = sprites[4];
        }
    }

    private void FixedUpdate()
    {
        if (!stunned)
        {
            float speedX, speedY;
            if (Mathf.Abs(movementInput.x) >= .5f)
            {
                speedX = actualSpeed.x + Time.deltaTime * movementInput.x * speed;
            }
            else
            {
                if (Mathf.Abs(rb.velocity.x) < .5f)
                {
                    speedX = 0;
                }
                else
                {
                    speedX = Mathf.Lerp(actualSpeed.x, 0, (deceleration / Time.deltaTime));
                }

            }

            if (Mathf.Abs(movementInput.y) >= .5f)
            {
                speedY = actualSpeed.y + Time.deltaTime * movementInput.y * speed;
            }
            else
            {
                if (Mathf.Abs(rb.velocity.y) < .5f)
                {
                    speedY = 0;
                }
                else
                {
                    speedY = Mathf.Lerp(actualSpeed.y, 0, (deceleration / Time.deltaTime));
                }
            }

            speedX = Mathf.Clamp(speedX, -speedMax, speedMax);
            speedY = Mathf.Clamp(speedY, -speedMax, speedMax);
            actualSpeed = new Vector2(speedX, speedY);
            rb.velocity = actualSpeed;
        }

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, -0.1f, transform.position.z);
            if (actualSpeed.y > 0)
            {
                actualSpeed.y = 0;
                rb.velocity = new Vector2(actualSpeed.x, 0);
            }
        }

    }



    public void Stun(Vector2 collisionDirection)
    {
        //controlsLocked = true;
        stunned = true;
        spotLight.SetActive(false);
        UIManager.i.cursorSprite.enabled = false;
        rb.velocity = Vector2.zero;
        actualSpeed = Vector2.zero;
        //movementInput = Vector2.zero;
        rb.AddForce(-collisionDirection * collisionForce);
        GameManager.i.ShakeScreen();
        StartCoroutine(StunTimer());
    }

    IEnumerator StunTimer()
    {
        float flickerSpeed = .1f;
        int tickQty = Mathf.RoundToInt(stunTime / flickerSpeed);
        int flip = 0;
        for (int i = 0; i < tickQty; i++)
        {
            if (flip == 0)
            {
                sr.material.SetColor("_TintAdd", Color.white);
            }
            else
            {
                sr.material.SetColor("_TintAdd", Color.black);
            }

            flip = (flip + 1) % 2;

            yield return new WaitForSeconds(flickerSpeed);
        }
        sr.material.SetColor("_TintAdd", Color.black);
        //yield return new WaitForSeconds(stunTime);
        actualSpeed = rb.velocity;
        //controlsLocked = false;
        stunned = false;
        spotLight.SetActive(true);
        UIManager.i.cursorSprite.enabled = true;
    }
}
