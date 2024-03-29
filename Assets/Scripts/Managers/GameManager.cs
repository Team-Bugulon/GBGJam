using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum DayTime
    {
        Day,
        Night,
        Evening
    };


    public bool gamepad = false;

    [Header("Gameplay")]
    public float uncovered = 0; //actualizes at boat
    public float uncoveredReal = 0; //real = always true
    public float lifetimeMax = 5f;
    public float lifetime = 0f;
    public bool playerUnderwater;
    public DayTime timeOfDay;

    [Header("References")]
    public Player player;
    public GameObject flash;
    public Light2D globalLight;
    public Light2D spotLight;
    public Light2D circleLight;
    public GameObject boatArrow;

    [Header("Tiles")]
    public List<Sprite> tilesSprites;



    bool gameover = false;
    bool win = false;

    public void Flash()
    {
        flash.SetActive(true);
        Invoke("StopFlash", 0.1f);
        UIManager.i.Snap();
        UIManager.i.cursorSprite.sprite = UIManager.i.cursorSprites[1];

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

        if (uncovered != uncoveredReal)
        {
            UIManager.i.boatui.gameObject.SetActive(true);
            GameManager.i.boatArrow.SetActive(true);
        }
    }

    public void StopFlash()
    {
        flash.SetActive(false);
        UIManager.i.cursorSprite.sprite = UIManager.i.cursorSprites[0];
    }

    private void Update()
    {
        if (gameover == false)
        {
            float lightIntensity = Mathf.Clamp((player.transform.position.y + 4) / (-10 * 6), 0, 1);
            globalLight.intensity = Mathf.Clamp(1 - lightIntensity, 0.05f, 1f);
            circleLight.intensity = lightIntensity;
            spotLight.intensity = .5f * lightIntensity;
        }
        //Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, -10);
    }

    private void Start()
    {
        SoundManager.i.BGFadeIn(0);
        SoundManager.i.BGFadeOut(1);
        boatArrow.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (player.transform.position.y < -4 && playerUnderwater == false)
        {
            playerUnderwater = true;
            SoundManager.i.BGFadeIn(1);
            SoundManager.i.BGFadeOut(0);
            UIManager.i.batteryTransform.transform.DOShakeRotation(.5f, .75f, 20).SetEase(Ease.OutCubic).SetLoops(-1);
            UIManager.i.ByeCounter();
        }
        else if (player.transform.position.y >= -4 && playerUnderwater == true)
        {
            playerUnderwater = false;
            SoundManager.i.BGFadeIn(0);
            SoundManager.i.BGFadeOut(1);
            UIManager.i.batteryTransform.transform.DOKill();
            //UIManager.i.batteryMain.transform.localPosition = new Vector2(-10.375f, 5f);
            UIManager.i.batteryTransform.localPosition = Vector2.zero;
            UIManager.i.batteryTransform.localRotation = Quaternion.identity;
            //UIManager.i.GiveBackPics();
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
            lifetime = Mathf.Max(0, lifetime - 10 * Time.deltaTime);
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

    public int score;
    void GameOver()
    {
        if (gameover == false && win == false)
        {
            gameover = true;

            SoundManager.i.MusicOut();
            SoundManager.i.BGFadeOut(0);
            SoundManager.i.BGFadeOut(1);

            score = Mathf.Max(50, TransitionManager.i.Level * 100 + Mathf.Clamp(Mathf.FloorToInt(uncoveredReal * 100), 0, 99));
            if (score > TransitionManager.i.BestLevel)
            {
                TransitionManager.i.BestLevel = score;
                SaveManager.i.Save();
            }

            //player.controlsLocked = true;
            player.GameOver();
            globalLight.intensity = 1;
            circleLight.intensity = 1;
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0, 2);

            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.Append(circleLight.transform.DOScale(.2f, 1.5f).SetEase(Ease.InOutQuad));
            sequence.AppendInterval(.5f);
            sequence.Append(circleLight.transform.DOScale(0, 1f).SetEase(Ease.InBack));
            sequence.OnComplete(() =>
                {
                    Time.timeScale = 0;
                    UIManager.i.GameOverScreen();
                    SoundManager.i.PlayMusic("GameOver");
                }
            );
        }
        
    }

    public void Win()
    {
        if (win == false)
        {
            win = true;
            SoundManager.i.BGFadeOut(0);
            SoundManager.i.BGFadeOut(1);
            player.Win();
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(.8f);
            //const float pixelSize = 1f / 64f;
            //sequence.Append(player.transform.parent.DOScale(2f, 1.75f).SetEase(Ease.InOutQuart).OnUpdate(() =>
            //{
            //    player.transform.parent.localScale = Mathf.RoundToInt(player.transform.parent.localScale.x / pixelSize) * pixelSize * Vector3.one;
            //}
            //));
            sequence.Append(player.transform.parent.DOScale(2f, 1.5f).SetEase(Ease.InOutQuart));

            sequence.OnComplete(() =>
            {
                //Time.timeScale = 0;
                TransitionManager.i.NextLevel();
            }
            );
        }
        
    }

    //public void Restart()
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    //}

    //public void Quit()
    //{
    //    Application.Quit();
    //}

    public void ShakeScreen(float duration = .35f, float strength = 5f, int vibrato = 20, DG.Tweening.Ease ease = Ease.OutExpo)
    {
        Camera.main.transform.parent.DOComplete();
        Camera.main.transform.parent.DOShakeRotation(duration, strength, vibrato).SetEase(ease).OnComplete(() => {
            //mainCamera.transform.parent.rotation = Quaternion.Euler(Vector3.zero);
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
            Camera.main.transform.position = new Vector3(0, Camera.main.transform.position.y, -10);
        });
    }

    public void Recharge()
    {
        lifetime = 0;
    }
}
