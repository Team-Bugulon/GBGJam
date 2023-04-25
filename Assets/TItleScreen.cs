using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleScreen : MonoBehaviour
{
    public GameObject mainBtns;
    public GameObject TutoCanvas;

    public List<GameObject> slides;

    int tutoID = 0;
    
    void Start()
    {
        Debug.Log("TitleScreen.Start()");
        SoundManager.i.PlayMusic("Title");
        TutoCanvas.SetActive(false);
        //embed package
        //UnityEditor.PackageManager.Client.Embed("com.unity.render-pipelines.universal");
    }

    public void TutoBtnNext()
    {
        tutoID++;
        
        if (tutoID > 4)
        {
            CloseTuto();
        } else
        {
            ActualizeTuto();
        }
    }
    
    public void SeeTuto()
    {
        tutoID = 0;
        mainBtns.SetActive(false);
        TutoCanvas.SetActive(true);
        ActualizeTuto();
    }
    
    public void CloseTuto()
    {
        mainBtns.SetActive(true);
        TutoCanvas.SetActive(false);
    }

    public void ActualizeTuto()
    {
        foreach (var slide in slides)
        {
            slide.SetActive(false);
        }

        slides[tutoID].SetActive(true);
        slides[tutoID].transform.localScale = Vector3.one;
        slides[tutoID].transform.DOShakeScale(.3f, .3f, 20);
    }
}
