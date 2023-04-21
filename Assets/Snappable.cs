using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Snappable : MonoBehaviour
{
    public UnityEvent onSnap;

    public void Snap()
    {
        onSnap.Invoke();
    }
}
