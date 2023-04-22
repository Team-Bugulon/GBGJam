using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public float cameraRadius;
    [SerializeField] CameraTrigger ct;

    Camera cameramanCamera;

    private void Start()
    {
        cameramanCamera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition = mousePosition - (Vector2)transform.parent.position;
        mousePosition = mousePosition - (Vector2)GameManager.i.player.transform.position;

        if ((mousePosition).magnitude > cameraRadius)
        {
            mousePosition = mousePosition.normalized * cameraRadius;
        }
        //transform.localPosition = mousePosition;
        transform.position = mousePosition + (Vector2)GameManager.i.player.transform.position;

        GameManager.i.player.spotLight.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg - 90);

        //if left click
        if (Input.GetMouseButtonDown(0) && !GameManager.i.player.stunned)
        {
            Snap();
        }
    }

    public void Snap()
    {
        cameramanCamera.gameObject.SetActive(true);
        cameramanCamera.Render();
        cameramanCamera.gameObject.SetActive(false);

        //copy list ct.ObjectsOnCamera
        List<Snappable> objectsOnCamera = new List<Snappable>();
        foreach (var item in ct.ObjectsOnCamera)
        {
            objectsOnCamera.Add(item);
        }

        foreach (var item in objectsOnCamera)
        {
            item.Snap();
        }
        
        GameManager.i.Flash();
    }
}
