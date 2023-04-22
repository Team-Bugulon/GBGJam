using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    private static GameManager _i;

    public static GameManager i
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

    public bool gamepad = false;

    [Header("Gameplay")]
    public float uncovered = 0; //actualizes at boat
    public float uncoveredReal = 0; //real = always true
    public float lifetimeMax = 5f;
    public float lifetime = 0f;
    public bool playerUnderwater;

    [Header("References")]
    public Player player;
    public GameObject flash;
    public Light2D globalLight;
    public Light2D spotLight;
    public Light2D circleLight;



    public void Flash()
    {
        flash.SetActive(true);
        Invoke("StopFlash", 0.1f);
        UIManager.i.Snap();

        //find all the objects with the tag mearl
        GameObject[] mearls = GameObject.FindGameObjectsWithTag("Mearl");
        int totalCount;
        int uncoveredCount;
        totalCount = mearls.Length;
        uncoveredCount = 0;
        foreach (GameObject mearl in mearls)
        {
            if (mearl.GetComponent<Mearl>().uncovered)
            {
                uncoveredCount++;
            }
        }
        uncoveredReal = (float)uncoveredCount / (float)totalCount;
    }

    public void StopFlash()
    {
        flash.SetActive(false);
    }

    private void Update()
    {
        float lightIntensity = Mathf.Clamp(player.transform.position.y / (-6 * 6), 0, 1);
        globalLight.intensity = Mathf.Clamp(1 - lightIntensity, 0.05f, 1f);
        circleLight.intensity = lightIntensity;
        spotLight.intensity = .5f * lightIntensity;
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y < -4 && playerUnderwater == false)
        {
            playerUnderwater = true;
            UIManager.i.batteryTransform.transform.DOShakeRotation(.5f, .75f, 20).SetEase(Ease.OutCubic).SetLoops(-1);
        }
        else if (player.transform.position.y >= -4 && playerUnderwater == true)
        {
            playerUnderwater = false;
            UIManager.i.batteryTransform.transform.DOKill();
            //UIManager.i.batteryMain.transform.localPosition = new Vector2(-10.375f, 5f);
            UIManager.i.batteryTransform.localPosition = Vector2.zero;
            UIManager.i.batteryTransform.localRotation = Quaternion.identity;
        }

        if (playerUnderwater)
        {
            lifetime += Time.deltaTime;
            if (lifetime > lifetimeMax)
            {
                GameOver();
            }
        }
        else
        {
            lifetime = 0;
        }

        float health = Mathf.Clamp(lifetime / lifetimeMax, 0, 1);
        UIManager.i.batteryAnchor.localScale = new Vector2(1, (Mathf.Floor((health * 1.625f) / .0625f) * .0625f) / 1.625f);
        if (health < .5f)
        {
            UIManager.i.batteryBG.color = Color.Lerp(UIManager.i.batteryColors[0], UIManager.i.batteryColors[1], health * 2);
        } else
        {
            UIManager.i.batteryBG.color = Color.Lerp(UIManager.i.batteryColors[1], UIManager.i.batteryColors[2], (health - .5f) * 2);
        }
        
    }

    void GameOver()
    {
        
    }
}
