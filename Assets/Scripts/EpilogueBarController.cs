using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class EpilogueBarController :  BottomBarController, IPointerEnterHandler, IPointerExitHandler
{

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        mouseOnBar = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        mouseOnBar = false;
    }

    public override bool IsMouseOnBar()
    {
        return false;
    }

    public override void ToggleClicking(bool turnOn)
    {

    }

    public override void UpdateMoneyButtons()
    {

    }

    private void Start()
    {

    }

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        
    }
}
