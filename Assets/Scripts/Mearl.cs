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
            //make material unique
            GetComponent<SpriteRenderer>().material = new Material(GetComponent<SpriteRenderer>().material);
            // #463D44
            GetComponent<SpriteRenderer>().material.SetColor("_TintAdd", new Color(0.27f, 0.24f, 0.27f, 1));
            // #D83C71
            sr[1].color = new Color(0.85f, 0.23f, 0.44f, 1);
            uncovered = true;
            transform.DOShakeScale(.5f, .3f, 20);
        }

    }
}
