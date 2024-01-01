using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class BottomBarController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    INPUTS clickInputs;
    GameObject recentClickTower;
    RubbleController recentSelectedRubble;
    float barTop;
    Camera mainCam;
    protected bool mouseOnBar = true;
    private const int removeRubblePrice = 100;
    [SerializeField] protected GridUtil gridScript;
    [SerializeField] protected Core coreScript;

    //rubble timer replenish visual effect
    [SerializeField] protected GameObject replenishTimerEffect;
    [SerializeField] protected Color replenishTimerColor;
    [SerializeField] protected Transform rubbleInfoPanel;

    private void Awake()
    {
        clickInputs = new INPUTS();
        clickInputs.Enable();
        clickInputs.TowerManipulation.GenericClickCheck.performed += CheckScreenClick;
    }

    private void OnDestroy()
    {
        clickInputs.TowerManipulation.GenericClickCheck.performed -= CheckScreenClick;
    }

    private GameObject shopDisplay;
    private GameObject towerDisplay;
    private GameObject rubbleDisplay;
    [SerializeField] protected Image towerImage;
    [SerializeField] protected TextMeshProUGUI upkeepPriceText;
    [SerializeField] protected TextMeshProUGUI sellValueText;
    [SerializeField] protected TextMeshProUGUI timeTillRubbleText;
    [SerializeField] protected TextMeshProUGUI towerNameText;
    [SerializeField] protected TextMeshProUGUI towerInfoText;
    [SerializeField] protected TextMeshProUGUI towerPriceText;
    [SerializeField] protected Button upkeepButton;
    [SerializeField] protected Button removeButton;
    private Coroutine rubbleTimerRoutine;
    private FirstLevelManager sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.Find("Scene Manager").GetComponent<FirstLevelManager>();
        shopDisplay = transform.Find("Buying Mode").gameObject;
        towerDisplay = transform.Find("Tower Mode").gameObject;
        rubbleDisplay = transform.Find("Rubble Mode").gameObject;
        mainCam = Camera.main;
        barTop = GetComponent<RectTransform>().rect.max.y * transform.parent.GetComponent<RectTransform>().localScale.y / Screen.height;
    }

    //called by tower buttons
    public void SwitchToTowerInfoDisplay(GameObject towerPrefab)
    {
        shopDisplay.SetActive(false);
        rubbleDisplay.SetActive(false);
        towerDisplay.SetActive(true);
        towerImage.sprite = towerPrefab.GetComponent<SpriteRenderer>().sprite;
        towerNameText.text = towerPrefab.GetComponent<TowerController>().TowerName;
        towerInfoText.text = towerPrefab.GetComponent<TowerController>().TowerDescription;
        towerPriceText.text = towerPrefab.GetComponent<TowerController>().Price.ToString();
        upkeepPriceText.transform.parent.gameObject.SetActive(false);
        sellValueText.transform.parent.gameObject.SetActive(false);
        timeTillRubbleText.text = towerPrefab.GetComponent<TowerController>().maxRubbleTime.ToString();
        if (recentClickTower != null)
        {
            recentClickTower.GetComponent<TowerController>().TowerDeselected();
        }
    }

    //Called by tower manager
    public void SwitchToTowerDisplay(GameObject tower)
    {
        shopDisplay.SetActive(false);
        rubbleDisplay.SetActive(false);
        towerDisplay.SetActive(true);
        towerImage.sprite = tower.GetComponent<SpriteRenderer>().sprite;
        towerNameText.text = tower.GetComponent<TowerController>().TowerName;
        towerInfoText.text = tower.GetComponent<TowerController>().TowerDescription;
        towerPriceText.text = tower.GetComponent<TowerController>().Price.ToString();
        upkeepPriceText.transform.parent.gameObject.SetActive(true);
        sellValueText.transform.parent.gameObject.SetActive(true);
        upkeepPriceText.text = "(-" + tower.GetComponent<TowerController>().UpkeepPrice + ")";
        sellValueText.text = "(+" + tower.GetComponent<TowerController>().SellValue + ")";
        StopAllCoroutines();
        StartCoroutine(UpdateRubbleTimerLive(tower));
        if (recentClickTower != null)
        {
            recentClickTower.GetComponent<TowerController>().TowerDeselected();
        }
        recentClickTower = tower;
        recentClickTower.GetComponent<TowerController>().TowerPressed();
        UpdateMoneyButtons();
    }

    public void SwitchToRubbleDisplay(RubbleController rubble)
    {
        recentSelectedRubble = rubble;
        shopDisplay.SetActive(false);
        towerDisplay.SetActive(false);
        rubbleDisplay.SetActive(true);
        if (recentClickTower != null)
        {
            recentClickTower.GetComponent<TowerController>().TowerDeselected();
        }
        UpdateMoneyButtons();
    }

    public void SwitchToShopDisplay()
    {
        StopAllCoroutines();
        towerDisplay.SetActive(false);
        rubbleDisplay.SetActive(false);
        shopDisplay.SetActive(true);
        if (recentClickTower != null)
        {
            recentClickTower.GetComponent<TowerController>().TowerDeselected();
        }
    }

    //Methods for buttons
    public void UpkeepPressed()
    {
        if (!FirstLevelManager.upkeepAllowed)
        {
            return;
        }
        TowerController selectedTower = recentClickTower.GetComponent<TowerController>();
        if (coreScript.CanAfford(selectedTower.UpkeepPrice))
        {
            coreScript.SpendEnergy(selectedTower.UpkeepPrice);
            PlayUpkeepAnim();
            selectedTower.UpkeepTower();
        }
        if (FirstLevelManager.tutorialActive)
        {
            sceneManager.UpkeepPressedResponse();
        }
    }
    public void SellPressed() //for tower
    {
        if (FirstLevelManager.tutorialActive)
        {
            return;
        }
        TowerController selectedTower = recentClickTower.GetComponent<TowerController>();
        selectedTower.SellTower();
        coreScript.GainEnergy(selectedTower.SellValue);

    }
    public void RemovePressed() //for rubble
    {
        if (coreScript.CanAfford(removeRubblePrice))
        {
            coreScript.SpendEnergy(removeRubblePrice);
            recentSelectedRubble.RemoveSelf();
            SwitchToShopDisplay();
        }
    }
    public void BackPressed()
    {
        SwitchToShopDisplay();
    }

    private void PlayUpkeepAnim()
    {
        GameObject animationInstance = Instantiate(replenishTimerEffect, rubbleInfoPanel);
        TextMeshProUGUI energyChangeText = animationInstance.GetComponent<TextMeshProUGUI>();
        energyChangeText.color = replenishTimerColor;
        energyChangeText.text = "+10";
        coreScript.CallBottomBarRoutine(animationInstance);
    }

    public IEnumerator DestroyEnergyAnimOnDelay(GameObject animObject)
    {
        yield return new WaitForSeconds(1); //1 is the length of the animation
        Destroy(animObject);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        mouseOnBar = true;
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        mouseOnBar = false;
    }

    void CheckScreenClick(InputAction.CallbackContext ctx)
    {
        Vector2 mousePos2D = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        TowerController towerClicked = gridScript.GetTowerOnTile(gridScript.ConvertPositionToTile(mousePos2D));
        if (towerClicked != null)
        {
            if (towerClicked.GetComponent<RubbleController>() == null)
            {
                SwitchToTowerDisplay(towerClicked.gameObject);
            }
            else
            {
                SwitchToRubbleDisplay(towerClicked.GetComponent<RubbleController>());
            }
        }
        if (towerClicked == null && !mouseOnBar)
        {
            SwitchToShopDisplay();
        }
    }

    public virtual bool IsMouseOnBar()
    {
        return mouseOnBar;
    }

    IEnumerator UpdateRubbleTimerLive(GameObject tower)
    {
        while (true)
        {
            if (tower == null)
            {
                SwitchToShopDisplay();
                yield break;
            }
            timeTillRubbleText.text = tower.GetComponent<TowerController>().timeTillRubble.ToString();
            yield return new WaitForSeconds(0.01f);
        }
    }

    //called by core when energy changes; also called by this when mode switches
    public virtual void UpdateMoneyButtons()
    {
        removeButton.interactable = coreScript.CanAfford(removeRubblePrice);
        if (recentClickTower == null)
        {
            return;
        }
        upkeepButton.interactable = coreScript.CanAfford(recentClickTower.GetComponent<TowerController>().UpkeepPrice);
    }

    public virtual void ToggleClicking(bool turnOn)
    {
        if (turnOn)
        {
            EnableClicking();
        }
        else
        {
            DisableClicking();
        }
    }
    private void DisableClicking()
    {
        clickInputs.TowerManipulation.GenericClickCheck.performed -= CheckScreenClick;
        SwitchToShopDisplay();
    }
    private void EnableClicking()
    {
        clickInputs.TowerManipulation.GenericClickCheck.performed += CheckScreenClick;
    }
}
