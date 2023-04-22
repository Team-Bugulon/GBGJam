using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mearl : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    SpriteRenderer[] sr;

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
        Debug.Log("Snap!");
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
