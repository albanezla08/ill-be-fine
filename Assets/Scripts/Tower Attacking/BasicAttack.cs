using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : TowerAttackType
{
    private bool attacking = false;
    private Coroutine active_monkey;
    private GameObject attackTarget;
    [SerializeField] private GameObject bulletVisualPrefab;
    [SerializeField] private Transform towerCenterTransform;
    private GameObject bulletVisual;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float bulletSpeed;
    private List<CoffeeBoostScript> coffeeBoosters = new List<CoffeeBoostScript>();

    private void Start()
    {
        bulletVisual = Instantiate(bulletVisualPrefab);
        bulletVisual.transform.SetParent(transform);
        bulletVisual.SetActive(false);
    }
    public override void TryToAttack(GameObject attackTarget)
    {
        this.attackTarget = attackTarget;
        if (!attacking)
        {
            active_monkey = StartCoroutine(Monkey_Behaviour());
        }
    }
    public override void BoostSpeed(CoffeeBoostScript booster)
    {
        if (coffeeBoosters.Contains(booster))
        {
            return;
        }
        coffeeBoosters.Add(booster);
        attackDelay /= 1.5f;
        bulletSpeed *= 1.5f;
    }
    public override void UnboostSpeed(CoffeeBoostScript booster)
    {
        if (!coffeeBoosters.Contains(booster))
        {
            return;
        }
        coffeeBoosters.Remove(booster);
        attackDelay *= 1.5f;
        bulletSpeed /= 1.5f;
    }
    //we can have multiple coroutines/methods for different upgrade levels of towers

    public IEnumerator Monkey_Behaviour()
    {
        attacking = true;
        yield return new WaitForSeconds(attackDelay);

        if (attackTarget != null)
        {
            attackTarget.GetComponent<move>().TakeDamage(1);
            StartCoroutine(AttackVisualEffect(Vector2.Distance(attackTarget.transform.position, towerCenterTransform.position)));
        }
        //we can add a visual effect here to make it look like a projectile is shot
        

        attacking = false;
        yield break;
    }

    IEnumerator AttackVisualEffect(float distanceToEnemy)
    {
        GameObject currentTarget = attackTarget;
        float travelTimer = 0;
        bulletVisual.SetActive(true);
        /*bulletVisual.transform.up = (attackTarget.transform.position - towerCenterTransform.position).normalized;*/
        bulletVisual.transform.position = towerCenterTransform.position;
        while (currentTarget != null && travelTimer < attackDelay - 0.1f && Vector2.Distance(bulletVisual.transform.position, currentTarget.transform.position) > 0.1f)
        {
            /*bulletVisual.transform.Translate(Vector2.up * Time.deltaTime * bulletSpeed);*/
            bulletVisual.transform.position = Vector2.MoveTowards(bulletVisual.transform.position, currentTarget.transform.position, bulletSpeed * Time.deltaTime);
            travelTimer += Time.deltaTime;
            yield return null;
        }
        bulletVisual.SetActive(false);
    }
}
