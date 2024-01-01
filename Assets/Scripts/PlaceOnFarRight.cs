using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceOnFarRight : MonoBehaviour
{
    private BoxCollider2D bc;
    public float shift;
    public float shrink;

   
    // Start is called before the first frame update
    void Start()
    {
        /*   
           mainCam = Camera.main;
           destinationPos = mainCam.ScreenToWorldPoint(new Vector2(mainCam.pixelWidth + shift, mainCam.pixelHeight / 2));
           destinationPos.x -= bc.bounds.size.x / 2;
           transform.position = destinationPos;
           bc.enabled = false;*/
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
    }
    private void Update()
    {

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(shift, Camera.main.pixelHeight / 2, 5));

      
        transform.localScale = new Vector3(Camera.main.orthographicSize * shrink / 3, (Camera.main.orthographicSize * shrink+3.5f)  / 1.1f, 0);
        
    }
}
