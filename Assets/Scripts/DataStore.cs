using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DataStore
{
    public static bool prevLevelWon;
    public static int chap1EndHealth;
    public static int chap2EndHealth;
    public static int chap3EndHealth;
    //these are UX settings
    public static bool pointAndClickTowerEnabled;
    public static event Action enablePointAndClickTower;
    public static bool pointAndClickCameraEnabled;
    public static event Action enablePointAndClickCamera;
    public static bool towerClickboxesEnabled;
    public static event Action enableTowerClickboxes;
    public static bool gridBoxesEnabled;
    public static event Action enableGridBoxes;

    public static int GetWorstLevel()
    {
        int lowestChapNum = 0;
        if (chap2EndHealth < chap1EndHealth && chap2EndHealth <= chap3EndHealth)
        {
            lowestChapNum = 1;
        }
        if (chap3EndHealth < chap1EndHealth && chap3EndHealth <= chap2EndHealth)
        {
            lowestChapNum = 2;
        }
        return lowestChapNum;
    }

    public static void ToggleClickBoxes()
    {
        towerClickboxesEnabled = !towerClickboxesEnabled;
        enableTowerClickboxes?.Invoke();
    }

    public static void ToggleCameraControls(bool isEnabled)
    {
        pointAndClickCameraEnabled = isEnabled;
        enablePointAndClickCamera?.Invoke();
    }

    public static void ToggleTowerControls(bool isEnabled)
    {
        pointAndClickTowerEnabled = isEnabled;
        enablePointAndClickTower?.Invoke();
    }
}
