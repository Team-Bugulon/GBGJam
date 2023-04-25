using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bodus : MonoBehaviour
{
    [SerializeField] int type = 0;
    void Start()
    {
        var pSr = transform.parent.GetComponent<SpriteRenderer>();
        int skin = 0;
        if (Mathf.Sin(transform.position.x) + Mathf.Sin(transform.position.y) > 0)
        {
            skin = 0;
        } else if (Mathf.Sin(transform.position.x) + Mathf.Sin(transform.position.y) <= -1)
        {
            skin = 2;
        } else
        {
            skin = 1;
        }

        pSr.sprite = GameManager.i.tilesSprites[type * 21 + skin];

        if (Random.Range(0,6) > 0)
        {
            this.gameObject.SetActive(false);
        } else
        {
            int babiole = Random.Range(0, 6);
            GetComponent<SpriteRenderer>().sprite = GameManager.i.tilesSprites[type * 21 + 3 + skin * 6 + babiole];
        }

        Destroy(this);
    }
}
