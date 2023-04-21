using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Player player;
    public GameObject flash;

    public void Flash()
    {
        flash.SetActive(true);
        Invoke("StopFlash", 0.1f);
    }

    public void StopFlash()
    {
        flash.SetActive(false);
    }
}
