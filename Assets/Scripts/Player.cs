using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Actions")]
    public float speed;
    public float speedMax;
    public float deceleration;
    [Header("Managment")]
    public bool controlsLocked;
    [Header("Items")]
    public GameObject spotLight;

    Vector2 movementInput;
    Vector2 actualSpeed;
    Rigidbody2D rb;
    BoxCollider2D cc;

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
        actualSpeed = Vector2.zero;
    }
    
    private void FixedUpdate()
    {
        float speedX, speedY;
        if (Mathf.Abs(movementInput.x) >= .5f)
        {
            speedX = actualSpeed.x + Time.deltaTime * movementInput.x * speed;
        } else
        {
            if (Mathf.Abs(rb.velocity.x) < .5f)
            {
                speedX = 0;
            } else {
                speedX = Mathf.Lerp(actualSpeed.x, 0, (deceleration / Time.deltaTime));
            }

        }
        
        if (Mathf.Abs(movementInput.y) >= .5f)
        {
            speedY = actualSpeed.y + Time.deltaTime * movementInput.y * speed;
        } else
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

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            if (actualSpeed.y < 0)
            {
                actualSpeed.y = 0;
                rb.velocity = new Vector2(actualSpeed.x, 0);
            }
        }
    }
}
