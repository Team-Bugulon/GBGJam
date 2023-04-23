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
                    break;
                case 1:
                    TransitionManager.i.MainMenu();
                    break;
            }
        }

    }
}
