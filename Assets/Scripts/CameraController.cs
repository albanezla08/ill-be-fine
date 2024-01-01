using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraControls clickMoveScript;
    private CameraMovementScript dragMoveScript;

    private void Awake()
    {
        DataStore.enablePointAndClickCamera += ChangeCameraMovement;
    }

    private void OnDestroy()
    {
        DataStore.enablePointAndClickCamera -= ChangeCameraMovement;
    }

    void Start()
    {
        clickMoveScript = GetComponent<CameraControls>();
        dragMoveScript = GetComponent<CameraMovementScript>();
        ChangeCameraMovement();
    }

    private void ChangeCameraMovement()
    {
        if (DataStore.pointAndClickCameraEnabled)
        {
            clickMoveScript.enabled = true;
            dragMoveScript.enabled = false;
        }
        else
        {
            clickMoveScript.enabled = false;
            dragMoveScript.enabled = true;
        }
    }
}
