using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mearl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onSnap()
    {
        Debug.Log("Snap!");
        GetComponent<SpriteRenderer>().color = Color.red;
    }
}
