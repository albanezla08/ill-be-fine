using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamageNoLimit : MonoBehaviour
{
    [HideInInspector] public TowerController towerOwner; //set by pennyRoller
    /*public static Action<float> DealDamageToEnemy;*/
    [HideInInspector] public int AttackPower; //set by pennyRoller
    [SerializeField] private int maxPops;
    private int popsLeft;

    private void Start()
    {
        popsLeft = maxPops;
    }

    public void SentToStartResponse()
    {
        Debug.Log("sent to tstart");
        popsLeft = maxPops;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            popsLeft--;
            collision.GetComponent<move>().TakeDamage(AttackPower);
            towerOwner.AddTargetsFromProjectile(collision.GetComponent<move>());
        }
        if (popsLeft <= 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
