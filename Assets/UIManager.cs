using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

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
        UIManager.i.boatui.gameObject.SetActive(false);
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
        givebackpicsbool = false;
    }

    void UpdatePercCount(int uncoveredPerc)
    {
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
}
