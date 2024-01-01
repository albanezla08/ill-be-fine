using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static bool startedSpawning = false; //made because i dont know how to code
    [SerializeField] Transform tutorialPath;
    [SerializeField] GameObject tutorialEnemy;
    private GameObject tutEnemyObj;
    [SerializeField] WaveType[] wavesToSpawn;
    private int currentWaveIndex = 0;

    [SerializeField] Core coreScript;
    private AudioManager audioManagerScript;

    private void OnDestroy()
    {
        startedSpawning = false;
    }

    public void TutorialSpawn()
    {
        Vector2[] temp = new Vector2[tutorialPath.childCount];
        for (int j = 0; j < temp.Length; j++)
        {
            temp[j] = tutorialPath.GetChild(j).position;
        }
        tutEnemyObj = Instantiate(tutorialEnemy, temp[0], Quaternion.identity);
        tutEnemyObj.GetComponent<FollowPoints>().SetPath(temp);
    }
    public void TutorialMove()
    {
        tutEnemyObj.GetComponent<move>().StartMoving();
    }

    public void StartSpawning()
    {
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        StartCoroutine(SpawnWave(wavesToSpawn[currentWaveIndex]));
        startedSpawning = true;
        audioManagerScript.PlaySound("Dreams");
    }

    IEnumerator SpawnWave(WaveType currentWave)
    {
        if (currentWave.enemyAmount > 0)
        {
            for (int i = 0; i < currentWave.enemyAmount; i++)
            {
                GameObject enemyObj = Instantiate(currentWave.enemyPrefab, Vector3.up * 100, Quaternion.identity);
                Vector2[] temp = new Vector2[currentWave.pathToSpawnOn.childCount];
                for (int j = 0; j < temp.Length; j++)
                {
                    temp[j] = currentWave.pathToSpawnOn.GetChild(j).position;
                }
                enemyObj.GetComponent<FollowPoints>().SetPath(temp);
                enemyObj.GetComponent<move>().StartMoving();
                yield return new WaitForSeconds(currentWave.enemySpawnDelay);
            }
        }
        else
        {
            yield return new WaitForSeconds(currentWave.enemySpawnDelay);
        }
        currentWaveIndex++;
        if (currentWaveIndex < wavesToSpawn.Length)
        {
            StartCoroutine(SpawnWave(wavesToSpawn[currentWaveIndex]));
        }
        else
        {
            print("stopped spawning");
            coreScript.EnemiesDoneSpawning();
        }
    }
}
