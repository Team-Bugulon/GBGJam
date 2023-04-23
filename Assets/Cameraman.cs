using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameraman : MonoBehaviour
{
    public float cameraRadius;
    [SerializeField] float CameraCooldown;
    
    [SerializeField] CameraTrigger ct;

    Camera cameramanCamera;
    float CameraTimer;

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
        
        CameraTimer += Time.deltaTime;
        //if left click
        if (Input.GetMouseButtonDown(0) && !GameManager.i.player.stunned && !GameManager.i.player.controlsLocked && CameraTimer >= CameraCooldown)
        {
            CameraTimer = 0;
            Snap();
        }
    }

    public void Snap()
    {
        SoundManager.i.Play("Camera", .15f, .8f);
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
