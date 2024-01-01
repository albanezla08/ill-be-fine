using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAttack : TowerAttackType
{
    [SerializeField] float attackDelay;
    [SerializeField] GameObject projectile;
    private bool isAttacking;
    private List<CoffeeBoostScript> coffeeBoosters = new List<CoffeeBoostScript>();

    public override void TryToAttack(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            StartCoroutine(RefreshCooldown());
            print(attackTarget);
            ShootProjectile(attackTarget.transform, towerOwner.damage);
        }
    }
    public override void BoostSpeed(CoffeeBoostScript boosterScript)
    {
        if (coffeeBoosters.Contains(boosterScript))
        {
            return;
        }
        coffeeBoosters.Add(boosterScript);
        attackDelay /= 1.5f;
    }
    public override void UnboostSpeed(CoffeeBoostScript boosterScript)
    {
        if (!coffeeBoosters.Contains(boosterScript))
        {
            return;
        }
        coffeeBoosters.Remove(boosterScript);
        attackDelay *= 1.5f;
    }

    public void ShootProjectile(Transform attackTarget, int damage)
    {
        Vector2 projectileUp = (attackTarget.position - transform.position).normalized;
      //  projectile.GetComponent<ConeProjectile>().moveDir = projectileUp;
        var rotation = GetOrientationToTarget(transform.position, attackTarget.position);
       
        GameObject projectile = Instantiate(this.projectile, transform.position, rotation);
        projectile.GetComponent<ConeProjectile>().towerOwner = base.towerOwner;
        projectile.GetComponent<ConeProjectile>().damage = damage;
        projectile.GetComponent<ConeProjectile>().attackTarget = attackTarget;


     

    }

    IEnumerator RefreshCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    protected static Quaternion GetOrientationToTarget(Vector3 position, Vector3 targetLocation)
    {
        var towerToTarget = targetLocation - position;
        var angle = Vector3.SignedAngle(Vector3.up, towerToTarget, Vector3.forward);
        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        return rotation;
    }
}
