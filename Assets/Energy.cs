using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.i.Recharge();
            SoundManager.i.Play("BatteryPickup", 0f, .8f);
            Destroy(this.gameObject);
        }
    }
}
