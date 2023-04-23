using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _i;

    public static SoundManager i
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
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] GameObject sourcePrefab;
    [SerializeField] List<AudioClip> audios;
    Dictionary<string, AudioClip> audioClips;

    [SerializeField] List<AudioClip> musics;
    Dictionary<string, AudioClip> musicsClips;

    AudioSource aSource;

    [SerializeField] AudioSource loopScore;
    public AudioSource moveLoop;
    public AudioSource surfaceBG;
    public AudioSource waterBG;
    [SerializeField] AudioSource music;

    string musicPlaying = "";

    public void BGFadeIn(int id = 0)
    {
        if (id == 0)
        {
            surfaceBG.DOKill();
            surfaceBG.DOFade(.15f, 1).SetEase(Ease.Linear).SetUpdate(true);
        } else
        {
            waterBG.DOKill();
            waterBG.DOFade(.2f, 1).SetEase(Ease.Linear).SetUpdate(true);
        }
    }

    public void BGFadeOut(int id = 0)
    {
        if (id == 0)
        {
            surfaceBG.DOKill();
            surfaceBG.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true);
        }
        else
        {
            waterBG.DOKill();
            waterBG.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true);
        }
    }

    // Start is called before the first frame update
    void Init()
    {
        audioClips = new Dictionary<string, AudioClip>();

        foreach (var audio in audios)
        {
            audioClips[audio.name] = audio;
        }

        musicsClips = new Dictionary<string, AudioClip>();

        foreach (var audio in musics)
        {
            musicsClips[audio.name] = audio;
        }

        aSource = GetComponent<AudioSource>();
    }

    public void Play(string name, float pitchRange = 0f, float volume = 1f)
    {
        var source = Instantiate(sourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        source.transform.parent = transform;

        source.clip = audioClips[name];
        source.pitch = 1f + Random.Range(-pitchRange, pitchRange);
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, audioClips[name].length + .5f);
    }

    public void PlayPitch(string name, float fixedPitch = 1f, float volume = 1f)
    {
        var source = Instantiate(sourcePrefab, Vector3.zero, Quaternion.identity).GetComponent<AudioSource>();
        source.transform.parent = transform;

        source.clip = audioClips[name];
        source.pitch = fixedPitch;
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, audioClips[name].length + .5f);
    }

    public void PlayOneShot(string name, float pitchRange = 0f, float volume = 1f)
    {
        aSource.pitch = 1f + Random.Range(-pitchRange, pitchRange);
        aSource.volume = volume;
        aSource.PlayOneShot(audioClips[name]);
    }

    public void LoopScore(bool on = false)
    {
        if (on)
        {
            loopScore.Play();
        } else
        {
            loopScore.Stop();
        }
    }

    public void MusicIn()
    {
        music.DOKill();
        music.DOFade(.8f, 1).SetEase(Ease.Linear).SetUpdate(true);
    }
    public void MusicOut()
    {
        music.DOKill();
        music.DOFade(0, 2).SetEase(Ease.Linear).SetUpdate(true);
    }

    public void PlayMusic(string musicName)
    {
        Debug.Log("gusic " + musicName);
        if (music.volume <= .001f)
        {
            music.clip = musicsClips[musicName];
            music.Play();
            musicPlaying = musicName;
            MusicIn();
        }
        else if (musicPlaying != musicName)
        {
            Debug.Log("gusic " + musicName);
            music.DOKill();
            music.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                music.clip = musicsClips[musicName];
                music.Play();
                musicPlaying = musicName;
                MusicIn();
            });
            
        }
    }

}
