using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _i;

    public static TransitionManager i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i == null)
        {
            _i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [InspectorButton("NextLevel")]
    public bool nextlevel;

    [SerializeField] Transform transi;
    public int Level = 0;
    
    public void TransiIn()
    {
        transi.DOKill();
        transi.position = new Vector2(40, Camera.main.transform.position.y);
        transi.DOMoveX(0, 1.5f).SetEase(Ease.InOutCubic).SetUpdate(true);
    }

    public void TransiOut()
    {
        Time.timeScale = 1;
        transi.DOKill();
        transi.position = new Vector2(0, Camera.main.transform.position.y);
        transi.DOMoveX(-40, 1.5f).SetEase(Ease.InOutCirc).SetUpdate(true);
    }

    public void NextLevel()
    {
        LoadLevel(Level + 1);
    }


    public void LoadLevel(int level = 0)
    {
        Level = level;
        StartCoroutine(LoadLevelCoroutine());
    }

    IEnumerator LoadLevelCoroutine(int level = 0)
    {
        TransiIn();
        yield return new WaitForSecondsRealtime(2f);
        //reload scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //TransiOut();
    }
}
