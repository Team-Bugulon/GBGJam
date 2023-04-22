using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mearl : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    SpriteRenderer[] sr;

    public bool uncovered = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentsInChildren<SpriteRenderer>();

        int skin = Random.Range(0, 3);
        sr[0].sprite = sprites[skin];
        sr[1].sprite = sprites[skin + 3];
    }

    public void onSnap()
    {
        if (uncovered == false)
        {
            Debug.Log("Snap!");
            GetComponent<SpriteRenderer>().color = Color.red;
            uncovered = true;
            transform.DOShakeScale(.5f, .3f, 20);
        }

    }
}
