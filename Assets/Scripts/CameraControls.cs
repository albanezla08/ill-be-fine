using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    INPUTS cameraControls;
    public Vector2 ZoomDirection;
    public float MangificationSpeed;

    public float speed;
    public float interpVelocity;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;


    private void Update()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * speed;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }

       


    }



    private void Awake()
    {
        INPUTS thisUser = new INPUTS();
        cameraControls = thisUser;
        cameraControls.Enable();


        cameraControls.CameraControls.zoom.performed += Zoom_performed;
        cameraControls.CameraControls.relocate.performed += Relocate_performed;
        cameraControls.CameraControls.MaxFocus.performed += MaxFocus_performed;
        /*cameraControls.CameraControls.zoom.canceled += ctx => ZoomDirection = Vector2.zero;*/
    }

    private void OnDestroy()
    {
        cameraControls.CameraControls.zoom.performed -= Zoom_performed;
        cameraControls.CameraControls.relocate.performed -= Relocate_performed;
        cameraControls.CameraControls.MaxFocus.performed -= MaxFocus_performed;
    }

    private void MaxFocus_performed(InputAction.CallbackContext obj)
    {
        if (Camera.main.orthographicSize <= 5.3f)
        {
            Camera.main.orthographicSize = 10f;
        }
        else
        {
            Camera.main.orthographicSize = 5.3f;
        }
    }

    private void Relocate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        target.transform.position = new Vector3(pos.x,pos.y,1);
        
    }

    private void Zoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Camera.main.orthographicSize >= 5.30f)
        {
            Camera.main.orthographicSize += obj.ReadValue<Vector2>().normalized.y * MangificationSpeed * Time.deltaTime * -1;
        }
        else
        {
            Camera.main.orthographicSize = 5.31f;
            print("zoom locked");
        }
    }
}
