using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public List<Snappable> ObjectsOnCamera;

    private void Start()
    {
        ObjectsOnCamera = new List<Snappable>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if is not in list
        if (!ObjectsOnCamera.Contains(collision.gameObject.GetComponentInChildren<Snappable>()))
        {
            //add to list
            ObjectsOnCamera.Add(collision.gameObject.GetComponentInChildren<Snappable>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if is not in list
        if (!ObjectsOnCamera.Contains(collision.gameObject.GetComponentInChildren<Snappable>()))
        {
            //add to list
            ObjectsOnCamera.Add(collision.gameObject.GetComponentInChildren<Snappable>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if is in list
        if (ObjectsOnCamera.Contains(collision.gameObject.GetComponentInChildren<Snappable>()))
        {
            //remove from list
            ObjectsOnCamera.Remove(collision.gameObject.GetComponentInChildren<Snappable>());
        }
    }
}
