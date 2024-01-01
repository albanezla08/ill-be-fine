using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData
{
    //stores data of a tower, is not attached to any game objects
    public Vector2 gridPosition;
    public string towerName;

    public TowerData(Vector2 pos, string name)
    {
        gridPosition = pos;
        towerName = name;
    }
}
public class RubbleData
{
    public Vector2 gridPosition;
    public Vector2 pileSize;

    public RubbleData(Vector2 pos, Vector2 size)
    {
        gridPosition = pos;
        pileSize = size;
    }
}

public class TowerManager : MonoBehaviour
{
    public static List<TowerData> towersAtEndOfLevel = new List<TowerData>();
    public static List<RubbleData> rubbleAtEndOfLevel = new List<RubbleData>();
    private List<RubbleController> rubbleInScene = new List<RubbleController>();
    private List<TowerController> towersInScene = new List<TowerController>();
    private List<TowerController> towersToDestroy = new List<TowerController>();
    private List<move> enemiesTargeted = new List<move>();
    private List<move> enemiesReachCenter = new List<move>();
    private List<GameObject> enemiesToDestroy = new List<GameObject>();

    private int energy;

    [SerializeField] Core coreScript;
    [SerializeField] GridUtil gridScript;
    [SerializeField] BottomBarController bottomBarScript;
    [SerializeField] int energyGainedOnKill = 30;

    //Events
    /*public static Action<float> getDaBread;*/
    

    private void Awake()
    {
        energy = 100;
        /*UITowerButtonController.SpentDaBread += spentDaBread;*/
    }


    // Update is called once per frame
    void Update()
    {
        enemiesTargeted.Clear();
        DestroyTowers();
        foreach (TowerController tower in towersInScene)
        {
            tower.UpdateTargetsList();
            tower.PickTarget();
            tower.AttackTarget();
        }
        foreach (move enemy in enemiesTargeted)
        {
            /*Debug.Log("Enemy ID: " + enemy.see);*/
            if (!enemiesToDestroy.Contains(enemy.gameObject) && enemy.HP <= 0)
            {
                enemiesToDestroy.Add(enemy.gameObject);

                coreScript.GainEnergy(energyGainedOnKill);
            }
        }
        foreach (move enemy in enemiesReachCenter)
        {
            if (!enemiesToDestroy.Contains(enemy.gameObject))
            {
                enemiesToDestroy.Add(enemy.gameObject);
            }
        }
        enemiesReachCenter.Clear();
        foreach (GameObject enemyToDestroy in enemiesToDestroy)
        {
            coreScript.EnemyLeftMap();
            Destroy(enemyToDestroy);
        }
        enemiesToDestroy.Clear();
    }

    public TowerController GetFirstTowerInScene()
    {
        return towersInScene[0];
    }

    public void AddToTowersList(TowerController towerToAdd)
    {
        towersInScene.Add(towerToAdd);
    }

    public void AddToRubbleList(RubbleController rubbleToAdd)
    {
        rubbleInScene.Add(rubbleToAdd);
    }
    public void RemoveFromRubbleList(RubbleController rubbleToRemove)
    {
        rubbleInScene.Remove(rubbleToRemove);
    }

    public void QueueTowerDestroy(TowerController towerToDestroy)
    {
        towersToDestroy.Add(towerToDestroy);
    }

    private void DestroyTowers()
    {
        List<TowerController> destroyedTowers = new List<TowerController>();
        foreach (TowerController tower in towersToDestroy)
        {
            towersInScene.Remove(tower);
            destroyedTowers.Add(tower);
        }
        foreach (TowerController tower in destroyedTowers)
        {
            towersToDestroy.Remove(tower);
            Destroy(tower.gameObject);
        }
    }

    public void AddToEnemiesList(move enemyToAdd)
    {
        if (!enemiesTargeted.Contains(enemyToAdd))
        {
            enemiesTargeted.Add(enemyToAdd);
        }
       // Debug.Log("Amount of enemies targeted: " + enemiesTargeted.Count);
    }

    public void QueueEnemyToDestroy(move enemyToDestroy)
    {
        enemiesReachCenter.Add(enemyToDestroy);
    }

    //Called by tower controllers
    public void ChangeBarDisplay(GameObject tower)
    {
        bottomBarScript.SwitchToTowerDisplay(tower);
    }

    //called at the end of a level to carry over towers
    public void LogMapState()
    {
        foreach (TowerController tower in towersInScene)
        {
            TowerData currentData = new TowerData(gridScript.ConvertPositionToTile(tower.transform.position), tower.TowerName);
            towersAtEndOfLevel.Add(currentData);
        }
        foreach (RubbleController rubble in rubbleInScene)
        {
            rubbleAtEndOfLevel.Add(rubble.RubbleInfo);
        }
    }
    //called at the start of a level to clear data
    public void ResetMapState()
    {
        towersAtEndOfLevel.Clear();
        rubbleAtEndOfLevel.Clear();
    }
}
