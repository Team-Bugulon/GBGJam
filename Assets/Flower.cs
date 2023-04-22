using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flower : MonoBehaviour
{
    [SerializeField] float reloadCooldown = 10f;

    bool dead = false;

    [SerializeField] GameObject energy;

    public void onSnap()
    {
        if (!dead){
            dead = true;
            //spawn energy
            var summoned = Instantiate(energy, transform.position, Quaternion.identity);
            summoned.transform.DOMoveY(transform.position.y + 1.5f, 1f).SetEase(Ease.OutCubic);
            transform.DOComplete();
            transform.DOShakeScale(.6f, .5f, 20);
            GetComponent<Animator>().Play("flower_dead");
            Invoke("reload", reloadCooldown);
        }
    }

    void reload()
    {
        GetComponent<Animator>().Play("flower_idle");
        dead = false;
        transform.DOComplete();
        transform.DOShakeScale(.4f, .5f, 20);
    }
}
