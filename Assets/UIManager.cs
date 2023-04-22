using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("References")]
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

    public int photoQty = 0;

    private void Start()
    {
        photo1.gameObject.SetActive(false);
        photo2.gameObject.SetActive(false);
        photo3.gameObject.SetActive(false);
    }

    public void Snap()
    {
        photoQty = Mathf.Min(3, photoQty + 1);
        
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

        photo1.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        photo2.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        photo3.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));

        quad.transform.rotation = Quaternion.Euler(0, 0, 0);

        photo1.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        photo2.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
        photo3.transform.DOShakeRotation(.5f, 30, 10).SetEase(Ease.OutCubic);
    }

}
