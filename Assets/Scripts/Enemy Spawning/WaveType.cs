using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveType
{
    public GameObject enemyPrefab;
    public int enemyAmount;
    public float enemySpawnDelay;
    /*public Vector3 spawnLocation;*/
    public Transform pathToSpawnOn;
}