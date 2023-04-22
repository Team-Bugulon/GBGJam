using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Urchin : MonoBehaviour
{
    public float stunDuration = 3f;

    public void onSnap()
    {
        var stunEnemies = GetComponent<StunEnemies>();
        if (!stunEnemies.stunned)
        {
            stunEnemies.stunned = true;
            GetComponent<Animator>().Play("urchin_stun");
            transform.DOComplete();
            transform.DOShakeScale(.6f, .5f, 20);
            Invoke("Unstun", stunDuration);

        }
    }

    void Unstun()
    {
        GetComponent<StunEnemies>().stunned = false;
        transform.DOComplete();
        transform.DOShakeScale(.4f, .5f, 20);
        GetComponent<Animator>().Play("urchin_idle");
    }
}
