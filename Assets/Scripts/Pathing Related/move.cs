using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    Guid id;
    public float speed;
    public string see;
    [SerializeField] private int hp = 3;
    public int AttackPower;

    public int HP { get { return hp; } }

    private TowerManager towerManager;
    private FollowPoints movementScript;
    private Transform ownSpriteTransform;
    private SpriteRenderer ownSpriteRenderer;
    private Coroutine moveRoutine;
    private AudioManager audioManagerScript;

    //states of health
    [SerializeField] private GameObject hpState2;
    [SerializeField] private GameObject hpState1;

    //Events
    public static Action<int> DealDamage;

    //called by enemySpawner
    public void StartMoving()
    {
        movementScript = GetComponent<FollowPoints>();
        ownSpriteTransform = transform.GetChild(0);
        ownSpriteRenderer = ownSpriteTransform.GetComponent<SpriteRenderer>();
        id = Guid.NewGuid();
        see = id.ToString();
        towerManager = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        GameObject.Find("Core").GetComponent<Core>().EnemyEnteredMap();
        movementScript.PlaceObjectOnPoint(transform, 0, ChangeDirectionResponse);
        moveRoutine = StartCoroutine(MoveForward());
    }

    private IEnumerator MoveForward()
    {
        while (true)
        {
            movementScript.MoveLocalUp(transform, speed, ChangeDirectionResponse, AttackTower);
            yield return null;
        }
    }

    public void AttackTower()
    {
        Debug.Log("Attacked center");
        DealDamage?.Invoke(hp);
        hp = 0;

        StopCoroutine(moveRoutine);
        towerManager.QueueEnemyToDestroy(this);
    }

    public void ChangeDirectionResponse()
    {
        ownSpriteTransform.up = Vector2.up;
    }

    public void TakeDamage(int damage)
    {
        audioManagerScript.PlaySound("Enemy Hurt");
        hp -= damage;
        if (hp == 2)
        {
            ownSpriteRenderer.sprite = hpState2.transform.GetComponentInChildren<SpriteRenderer>().sprite;
            speed = hpState2.GetComponent<move>().speed;
        }
        else if (hp == 1)
        {
            ownSpriteRenderer.sprite = hpState1.transform.GetComponentInChildren<SpriteRenderer>().sprite;
            speed = hpState1.GetComponent<move>().speed;
        }
    }
}
