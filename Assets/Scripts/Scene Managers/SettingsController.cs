using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Color boxCheckedColor;
    [SerializeField] private Color boxUncheckedColor;
    [SerializeField] private Image camLeftCheckBox;
    [SerializeField] private Image camRightCheckBox;
    [SerializeField] private TextMeshProUGUI camInfo;
    [SerializeField] private string pointClickCamInfo;
    [SerializeField] private string dragCamInfo;
    [SerializeField] private Image towerLeftCheckBox;
    [SerializeField] private Image towerRightCheckBox;
    [SerializeField] private TextMeshProUGUI towerInfo;
    [SerializeField] private string pointClickTowerInfo;
    [SerializeField] private string dragTowerInfo;

    public void OpenSettingsMenu()
    {
        if (settingsPanel.activeInHierarchy)
        {
            CloseSettingsMenu();
            return;
        }
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
        if (DataStore.pointAndClickCameraEnabled)
        {
            CamPointClickPressed();
        }
        else
        {
            CamDragPressed();
        }
        if (DataStore.pointAndClickTowerEnabled)
        {
            TowerPointClickPressed();
        }
        else
        {
            TowerDragPressed();
        }
    }
    public void CloseSettingsMenu()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
    }

    public void CamPointClickPressed()
    {
        camLeftCheckBox.color = boxCheckedColor;
        camRightCheckBox.color = boxUncheckedColor;
        camInfo.text = pointClickCamInfo;
        DataStore.ToggleCameraControls(true);
    }
    public void CamDragPressed()
    {
        camLeftCheckBox.color = boxUncheckedColor;
        camRightCheckBox.color = boxCheckedColor;
        camInfo.text = dragCamInfo;
        DataStore.ToggleCameraControls(false);
    }

    public void TowerPointClickPressed()
    {
        towerLeftCheckBox.color = boxCheckedColor;
        towerRightCheckBox.color = boxUncheckedColor;
        towerInfo.text = pointClickTowerInfo;
        DataStore.ToggleTowerControls(true);
    }
    public void TowerDragPressed()
    {
        towerLeftCheckBox.color = boxUncheckedColor;
        towerRightCheckBox.color = boxCheckedColor;
        towerInfo.text = dragTowerInfo;
        DataStore.ToggleTowerControls(false);
    }
}
