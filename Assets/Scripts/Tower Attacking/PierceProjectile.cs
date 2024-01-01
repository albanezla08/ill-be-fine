using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceProjectile : TowerAttackType
{
    [SerializeField] float attackDelay;
    [SerializeField] GameObject projectile;
    private bool isAttacking;

    public override void TryToAttack(GameObject attackTarget)
    {
        if (!isAttacking)
        {
            StartCoroutine(RefreshCooldown());
            ShootProjectile(attackTarget.transform, towerOwner.damage);
        }
    }
    public override void BoostSpeed(CoffeeBoostScript booster)
    {
        //unused
    }
    public override void UnboostSpeed(CoffeeBoostScript booster)
    {
        //unused
    }

    private void ShootProjectile(Transform attackTarget, int damage)
    {
        Vector2 projectileUp = (attackTarget.position - transform.position).normalized;
        GameObject projectile = Instantiate(this.projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<PierceProjectileController>().towerOwner = base.towerOwner;
        projectile.GetComponent<PierceProjectileController>().moveDir = projectileUp;
        projectile.GetComponent<PierceProjectileController>().damage = damage;
    }

    IEnumerator RefreshCooldown()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
}
