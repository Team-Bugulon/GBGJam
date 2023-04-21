using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sound_Manager : MonoBehaviour
{
    private static Sound_Manager _i;

    public static Sound_Manager i
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

    [SerializeField] GameObject sourcePrefab;
    [SerializeField] List<AudioClip> audios;
    Dictionary<string, AudioClip> audioClips;

    AudioSource aSource;

    [SerializeField] AudioSource loopScore;
    [SerializeField] AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        audioClips = new Dictionary<string, AudioClip>();

        foreach (var audio in audios)
        {
            audioClips[audio.name] = audio;
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
        music.DOFade(0, 1).SetEase(Ease.Linear).SetUpdate(true);
    }

}
