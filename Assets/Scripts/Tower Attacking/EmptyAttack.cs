using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAttack : TowerAttackType
{
    //just for penny roller
    public override void TryToAttack(GameObject enemy)
    {
        //nothing
    }
    public override void BoostSpeed(CoffeeBoostScript booster)
    {
        //nothing
    }
    public override void UnboostSpeed(CoffeeBoostScript booster)
    {
        //nothing agian
    }
}
