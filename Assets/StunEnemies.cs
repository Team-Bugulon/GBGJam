using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemies : MonoBehaviour
{
    public bool stunned = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !stunned)
        {
            var direction = ((Vector2)(transform.position - collision.transform.position)).normalized;
            collision.gameObject.GetComponent<Player>().Stun(direction);
        }
    }
}
