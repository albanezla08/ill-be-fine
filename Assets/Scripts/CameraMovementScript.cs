using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private BottomBarController bottomBarScript;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;
    public Vector3 topLeft;
    public Vector3 bottomRight;

    // Update is called once per frame
    private void Update()
    {
        PanCamera();
        ZoomCamera();
    }

    private void PanCamera()
    {
        //Save the position of the mouse in world space when the drag starts (first time clicked)
        if (Input.GetMouseButtonDown(1) && !bottomBarScript.IsMouseOnBar())
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(DragCamera());
        }
    }

    private IEnumerator DragCamera()
    {
        while (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            //Move the camera by that distance
            if (topLeft.x > cam.transform.position.x + difference.x || bottomRight.x < cam.transform.position.x + difference.x)
            {
                difference.x = 0;
            }
            if (topLeft.y < cam.transform.position.y + difference.y || bottomRight.y > cam.transform.position.y + difference.y)
            {
                difference.y = 0;
            }
            cam.transform.position += difference;
            yield return null;
        }
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        /*if (newSize < minCamSize)
        {
            newSize = minCamSize;
        }*/
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        /*if (newSize > maxCamSize)
        {
            newSize = maxCamSize;
        }*/
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    }

    private void ZoomCamera()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            ZoomIn();
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            ZoomOut();
        }
    }
}
