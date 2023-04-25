using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Diagnostics.Tracing;
using static UnityEditor.PlayerSettings;

public class UIManager : MonoBehaviour
{
    private static UIManager _i;

    public static UIManager i
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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("References")]
    [Header("Cameraman")]
    public SpriteRenderer cursorSprite;
    public List<Sprite> cursorSprites;
    
    [Header("PhotoUI")]
    public SpriteRenderer photo1;
    public SpriteRenderer photo2;
    public SpriteRenderer photo3;
    public GameObject quad;

    [Header("Battery")]
    public Transform batteryTransform;
    public SpriteRenderer batteryMain;
    public SpriteRenderer batteryBG;
    public Transform batteryAnchor;
    public List<Color> batteryColors;

    [Header("Perc")]
    public Transform percTransform; //y = 2.5f down, 5.625f up
    public SpriteRenderer boatui;
    public SpriteRenderer digit1;
    public SpriteRenderer digit2;
    [SerializeField] List<Sprite> digits;

    public int photoQty = 0;

    private void Start()
    {
        photo1.gameObject.SetActive(false);
        photo2.gameObject.SetActive(false);
        photo3.gameObject.SetActive(false);

        percTransform.localPosition = new Vector3(percTransform.localPosition.x, 5.625f, percTransform.localPosition.z);
        boatui.gameObject.SetActive(false);
    }

    public void Snap()
    {
        //photoQty = Mathf.Min(3, photoQty + 1);
        photoQty += 1;

        if (photoQty == 1)
        {
            percTransform.DOKill();
            percTransform.DOLocalMoveY(2.5f, .5f).SetEase(Ease.OutCubic);
        }

        if (photoQty > 0)
        {
            photo1.gameObject.SetActive(true);
        }
        if (photoQty > 1)
        {
            photo2.gameObject.SetActive(true);
        }
        if (photoQty > 2)
        {
            photo3.gameObject.SetActive(true);
        }

        photo1.transform.DOKill();
        photo2.transform.DOKill();
        photo3.transform.DOKill();

        photo1.transform.localPosition = new Vector3(photo1.transform.localPosition.x, 10, photo1.transform.localPosition.z);
        photo1.transform.DOLocalMoveY(4, .25f).SetEase(Ease.OutCubic);

        photo2.transform.localPosition = new Vector3(9, photo2.transform.localPosition.y, photo2.transform.localPosition.z);
        photo2.transform.DOLocalMoveX(10, .25f).SetEase(Ease.OutCubic);

        photo3.transform.localPosition = new Vector3(10, photo3.transform.localPosition.y, photo3.transform.localPosition.z);
        photo3.transform.DOLocalMoveX(10.75f, .25f).SetEase(Ease.OutCubic);

        photo1.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));
        photo2.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));
        photo3.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));

        quad.transform.rotation = Quaternion.Euler(0, 0, 0);

        photo1.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        photo2.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        photo3.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
    }

    public void GiveBackPic()
    {
        SoundManager.i.Play("PaperAlt", .1f, .8f);
        UIManager.i.boatui.gameObject.SetActive(false);
        GameManager.i.boatArrow.SetActive(false);
        photo1.gameObject.SetActive(false);
        photo2.gameObject.SetActive(false);
        photo3.gameObject.SetActive(false);
        if (photoQty > 0)
        {
            photo1.gameObject.SetActive(true);
        }
        if (photoQty > 1)
        {
            photo2.gameObject.SetActive(true);
        }
        if (photoQty > 2)
        {
            photo3.gameObject.SetActive(true);
        }



        photo1.transform.DOKill();
        photo2.transform.DOKill();
        photo3.transform.DOKill();

        photo1.transform.localPosition = new Vector3(photo1.transform.localPosition.x, 4, photo1.transform.localPosition.z);
        photo1.transform.DOLocalMoveY(10, .1f).SetEase(Ease.OutCubic);

        photo2.transform.localPosition = new Vector3(10, photo2.transform.localPosition.y, photo2.transform.localPosition.z);
        photo2.transform.DOLocalMoveX(9, .1f).SetEase(Ease.OutCubic);

        photo3.transform.localPosition = new Vector3(10.75f, photo3.transform.localPosition.y, photo3.transform.localPosition.z);
        photo3.transform.DOLocalMoveX(10, .1f).SetEase(Ease.OutCubic);

        photo1.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));
        photo2.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));
        photo3.transform.localRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10));

        quad.transform.rotation = Quaternion.Euler(0, 0, 0);
        
        photoQty -= 1;

        //if (photoQty == 0)
        //{
        //    percTransform.DOKill();
        //    percTransform.DOLocalMoveY(5.625f, .5f).SetEase(Ease.OutCubic);
        //}


        //photo1.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        //photo2.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        //photo3.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
    }

    public void GiveBackPics()
    {
        if (givebackpicsbool == false)
        {
            givebackpicsbool = true;
            photoQty = Mathf.Min(10, photoQty);
            if (GameManager.i.uncovered != GameManager.i.uncoveredReal) SoundManager.i.Play("Deposit", 0f, .8f);
            StartCoroutine(GiveBackPicsCoroutine());
        }
    }

    bool givebackpicsbool = false;

    IEnumerator GiveBackPicsCoroutine()
    {
        while (photoQty > 0)
        {
            GiveBackPic();
            yield return new WaitForSeconds(.1f);
        }

        int uncoveredPerc = Mathf.Clamp(Mathf.FloorToInt(GameManager.i.uncovered * 100), 0, 99);
        int uncoveredRealPerc = Mathf.Clamp(Mathf.FloorToInt(GameManager.i.uncoveredReal * 100), 0, 99);
        int diff = uncoveredRealPerc - uncoveredPerc;
        if (diff > 0)
        {
            float tickDuration = 1f / (uncoveredRealPerc - uncoveredPerc);
            for (int i = 0; i < diff; i++)
            {
                uncoveredPerc++;
                UpdatePercCount(uncoveredPerc);
                yield return new WaitForSeconds(tickDuration);
            }
        }


        percTransform.DOComplete();
        percTransform.DOLocalMoveY(5.625f, .5f).SetEase(Ease.OutCubic);

        GameManager.i.uncovered = GameManager.i.uncoveredReal;
        UIManager.i.boatui.gameObject.SetActive(false);
        GameManager.i.boatArrow.SetActive(false);
        givebackpicsbool = false;

        if (GameManager.i.uncovered >= .99f)
        {
            GameManager.i.Win();
        }
    }

    void UpdatePercCount(int uncoveredPerc)
    {
        SoundManager.i.Play("TickPercentage", .1f, .3f);
        percTransform.DOComplete();
        percTransform.DOPunchScale(Vector3.one * .25f, .1f, 10);
        string woubi = uncoveredPerc.ToString();
        if (woubi.Length == 1)
        {
            woubi = "0" + woubi;
        }

        digit1.sprite = digits[(int)Char.GetNumericValue(woubi[0])];
        digit2.sprite = digits[(int)Char.GetNumericValue(woubi[1])];
    }

    [Header("GameOver")]
    public SpriteRenderer GO_BG;
    public Transform GO_logo;
    public Transform GO_gobg;
    public Transform GO_text1;
    public Transform GO_text2;
    public Transform GO_Buttons;

    [Header("LevelCounter")]
    public Transform LevelCounter;

    bool byeCounter = false;
    public void ByeCounter()
    {
        const float pixelSize = 1f / 16f;
        if (byeCounter == false)
        {
            byeCounter = true;
            LevelCounter.DOLocalMoveY(4, 1.5f).SetEase(Ease.InBack).OnUpdate(() => LevelCounter.localPosition = new Vector2(LevelCounter.localPosition.x, Mathf.RoundToInt(LevelCounter.localPosition.y / pixelSize) * pixelSize));
        }
    }


    public void GameOverScreen()
    {
        GO_BG.color = new Color(0, 0, 0, 0);
        GO_logo.transform.localPosition = new Vector2(0, 9);
        GO_gobg.transform.localScale = Vector2.zero;
        GO_text1.gameObject.SetActive(false);
        GO_text2.gameObject.SetActive(false);
        GO_Buttons.transform.localPosition = new Vector2(0, -3.5f);
        GO_Buttons.gameObject.SetActive(false);
        GO_BG.transform.parent.gameObject.SetActive(true);
        StartCoroutine(GameOverScreenCoroutine());
    }

    IEnumerator GameOverScreenCoroutine()
    {
        yield return new WaitForSecondsRealtime(.25f);
        GO_BG.DOColor(new Color(0,0,0,.75f), 1f).SetUpdate(true);
        GO_logo.DOLocalMoveY(3.5f, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1.2f);
        GO_gobg.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);
        GO_text1.gameObject.SetActive(true);
        var text1 = GO_text1.GetComponentsInChildren<TMPro.TextMeshPro>();
        int boba = 0;
        int score = GameManager.i.score;
        DOTween.To(() => boba, x => { boba = x; text1[0].text = "Score:" + x.ToString(); text1[1].text = "Score:" + x.ToString(); SoundManager.i.Play("TickGameOver", .1f, .1f); }, score, 2).SetUpdate(true);
        yield return new WaitForSecondsRealtime(2.5f);
        GO_text2.gameObject.SetActive(true);
        var text2 = GO_text2.GetComponentsInChildren<TMPro.TextMeshPro>();
        int boba2 = 0;
        int score2 = TransitionManager.i.BestLevel;
        DOTween.To(() => boba2, x => { boba2 = x; text2[0].text = "Highscore:" + x.ToString(); text2[1].text = "Highscore:" + x.ToString(); SoundManager.i.Play("TickGameOver", .1f, .1f); }, score2, 1).SetUpdate(true);
        yield return new WaitForSecondsRealtime(1f);
        GO_Buttons.gameObject.SetActive(true);
        GO_Buttons.DOLocalMoveY(0, 1f).SetEase(Ease.OutCubic).SetUpdate(true);
    }
}
