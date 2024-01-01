using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Is not an attack; lasts forever
public class CoffeeBoostScript : MonoBehaviour
{
    private TowerController towerOwner;
    private float checkRadius;
    private List<GameObject> towersList = new List<GameObject>();
    private Coroutine checkForTowersRoutine;

    private void Start()
    {
        towerOwner = GetComponent<TowerController>();
        checkRadius = towerOwner.AttackRadius;
        checkForTowersRoutine = StartCoroutine(CheckForTowersLoop());
    }

    private void OnDestroy()
    {
        StopCoroutine(checkForTowersRoutine);
        CheckRadiusForTowers();
        UnboostTowersInList();
    }

    private void CheckRadiusForTowers()
    {
        Collider2D[] colliderArray = Physics2D.OverlapCircleAll(towerOwner.TowerCenterTransform.position, checkRadius);
        towersList.Clear();
        for (int i = 0; i < colliderArray.Length; i++)
        {
            if (colliderArray[i].GetComponent<TowerController>() != null)
            {
                towersList.Add(colliderArray[i].gameObject);
            }
        }
    }

    private void BoostTowersInList()
    {
        foreach (GameObject tower in towersList)
        {
            if (tower.GetComponent<PennyRoller>() != null)
            {
                tower.GetComponent<PennyRoller>().BoostSpeed(this);
            }
            else
            {
                tower.GetComponent<TowerAttackType>().BoostSpeed(this);
            }
        }
    }

    private void UnboostTowersInList()
    {
        foreach (GameObject tower in towersList)
        {
            if (tower.GetComponent<PennyRoller>() != null)
            {
                tower.GetComponent<PennyRoller>().UnboostSpeed(this);
            }
            else
            {
                tower.GetComponent<TowerAttackType>().UnboostSpeed(this);
            }
        }
    }

    private IEnumerator CheckForTowersLoop()
    {
        while (true)
        {
            CheckRadiusForTowers();
            BoostTowersInList();
            yield return new WaitForSeconds(1f);
        }
    }
}
