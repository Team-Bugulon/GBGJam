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

    Vector2 movementInput;
    Vector2 actualSpeed;
    Rigidbody2D rb;
    CircleCollider2D cc;

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
        cc = GetComponent<CircleCollider2D>();
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
            speedX = Mathf.Lerp(actualSpeed.x, 0, (deceleration / Time.deltaTime));
            //speedX = actualSpeed.x - Mathf.Sign(actualSpeed.x) * deceleration * Time.deltaTime;
            //speedX = 0;

        }
        
        if (Mathf.Abs(movementInput.y) >= .5f)
        {
            speedY = actualSpeed.y + Time.deltaTime * movementInput.y * speed;
        } else
        {
            //speedY = actualSpeed.y - Mathf.Sign(actualSpeed.y) * deceleration * Time.deltaTime;
            speedY = Mathf.Lerp(actualSpeed.y, 0, (deceleration / Time.deltaTime));
            //speedY = 0;
        }
        
        speedX = Mathf.Clamp(speedX, -speedMax, speedMax);
        speedY = Mathf.Clamp(speedY, -speedMax, speedMax);
        actualSpeed = new Vector2(speedX, speedY);
        rb.velocity = actualSpeed;
    }
}
