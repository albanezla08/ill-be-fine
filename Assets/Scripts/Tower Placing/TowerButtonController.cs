using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButtonController : MonoBehaviour
{
/*    [SerializeField] private GameObject correspondingTower;
    [SerializeField] private GameObject correspondingTowerMarker;
    private SpriteRenderer towerDisplayed;
    private Camera mainCam;
    public float bread;

    private void Awake()
    {
        TowerManager.getDaBread += acquireBread;
        bread = 1f;
    }

    void acquireBread(float money)
    {
        bread += money;
    }

    private void Start()
    {
        mainCam = Camera.main;
        towerDisplayed = transform.GetChild(0).GetComponent<SpriteRenderer>();
        towerDisplayed.sprite = correspondingTower.GetComponent<SpriteRenderer>().sprite;

    }

    private void OnMouseDown()
    {
        //instantiate tower marker at MousePos
        if (bread >= 1)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            GameObject towerMarkerInstance = Instantiate(correspondingTowerMarker, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
            towerMarkerInstance.GetComponent<TowerMarkerController>().correspondingTowerPrefab = correspondingTower;
            bread--;
            spentDaBread?.Invoke(1);
        }
        else
        {
            print($"nah you broke");
        }
    }

    public static Action<float> spentDaBread;*/
}
