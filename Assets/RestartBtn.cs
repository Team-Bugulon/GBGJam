using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBtn : MonoBehaviour
{
    public int type = 0;
    public bool oneTime = true;
    bool clicked = false;
    public void onClick()
    {
        if ((oneTime && clicked == false) || !oneTime)
        {
            clicked = true;
            switch (type)
            {
                case 0:
                    TransitionManager.i.LoadLevel(0);
                    SoundManager.i.Play("StartGame", 0f, .7f);
                    break;
                case 1:
                    TransitionManager.i.MainMenu();
                    SoundManager.i.Play("MenuSelect", .15f, .8f);
                    break;
                case 2:
                    TransitionManager.i.LoadLevel(0);
                    SoundManager.i.Play("StartGame", 0f, .7f);
                    break;
                case 3:
                    SoundManager.i.Play("MenuSelect", .15f, .8f);
                    break;
            }
        }

    }
}
