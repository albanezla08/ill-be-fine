using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UITowerButtonController : MonoBehaviour
    , IPointerDownHandler
{
    /*public static Action<int> SpentDaBread;*/ //event called when money is spent

    [SerializeField] private GameObject correspondingTower;
    private Core coreScript;
    private BottomBarController bottomBarScript;

    private bool clickable = true;

    private void Start()
    {
        coreScript = GameObject.Find("Core").GetComponent<Core>();
        bottomBarScript = GameObject.Find("Bottom Bar").GetComponent<BottomBarController>();
        transform.GetChild(0).GetComponent<Image>().sprite = correspondingTower.GetComponent<SpriteRenderer>().sprite;
        transform.Find("Cost Text").GetComponent<TextMeshProUGUI>().text = correspondingTower.GetComponent<TowerController>().Price.ToString();
        /*StartCoroutine(CheckAfford());*/
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (!clickable)
        {
            return;
        }
        Instantiate(correspondingTower.GetComponent<TowerController>().towerMarker, transform.position, Quaternion.identity).GetComponent<TowerMarkerController>().correspondingTowerPrefab = correspondingTower;
        bottomBarScript.SwitchToTowerInfoDisplay(correspondingTower);
    }

    private void Update()
    {
        if (coreScript.CurrentEnergy < correspondingTower.GetComponent<TowerController>().Price)
        {
            GetComponent<Button>().interactable = false;
            clickable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
            clickable = true;
        }
    }

    /*IEnumerator CheckAfford()
    {
        while (true)
        {
            if (coreScript.CurrentEnergy < correspondingTower.GetComponent<TowerController>().Price)
            {
                GetComponent<Button>().interactable = false;
                clickable = false;
            }
            else
            {
                GetComponent<Button>().interactable = true;
                clickable = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }*/
}
