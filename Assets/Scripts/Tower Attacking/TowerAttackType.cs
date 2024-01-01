using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAttackType : MonoBehaviour
{
    public TowerController towerOwner;
    protected TowerManager towerManager;
    public abstract void TryToAttack(GameObject attackTarget);
    public abstract void BoostSpeed(CoffeeBoostScript boosterScript);
    public abstract void UnboostSpeed(CoffeeBoostScript boosterScript);
    private void Start()
    {
        towerManager = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
    }
}
