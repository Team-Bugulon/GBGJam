using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TItleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TitleScreen.Start()");
        SoundManager.i.PlayMusic("Title");
    }
}
