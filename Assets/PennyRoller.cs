using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PennyRoller : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] private Transform towerCenter;
    private int ActiveDirection;
    public List<GameObject> PossibleDirections = new List<GameObject>();
    INPUTS DirectionControls;
    [HideInInspector] public TowerManager towerManager; //set by towerController


    Transform[] attackPath = new Transform[1];

    private void Awake()
    {
        INPUTS thisUser = new INPUTS();
        DirectionControls = thisUser;
        DirectionControls.Enable();
        DirectionControls.TowerManipulation.SelectDirectionForPennyRoller.performed += SelectDirectionForPennyRoller_performed;
    }

    private void OnDestroy()
    {
        DirectionControls.TowerManipulation.SelectDirectionForPennyRoller.performed -= SelectDirectionForPennyRoller_performed;
    }

    private void SelectDirectionForPennyRoller_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit && EnemySpawner.startedSpawning)
        {
            if (hit.transform.parent != transform)
            {
                return;
            }
            /*ActiveDirection = Int32.Parse(hit.transform.name);
            HideArrows(hit.transform.name);*/
            switch (hit.transform.name)
            {
                case "1":
                    ActiveDirection = Int32.Parse(hit.transform.name);
                    HideArrows(hit.transform.name);
                    break;
                case "2":
                    ActiveDirection = Int32.Parse(hit.transform.name);
                    HideArrows(hit.transform.name);
                    break;
                case "3":
                    ActiveDirection = Int32.Parse(hit.transform.name);
                    HideArrows(hit.transform.name);
                    break;
                case "4":
                    ActiveDirection = Int32.Parse(hit.transform.name);
                    HideArrows(hit.transform.name);
                    break;
            }


        }
    }


    public void HideArrows(string name)
    {
        DirectionControls.TowerManipulation.Disable();
        GameObject targetPath = null;
        Debug.Log(name);
        foreach (Transform child in this.gameObject.transform)
        {
            if (!(child.name.Equals("Direction " + name) || child.name.Equals("Tower Selected Visual")))
                Destroy(child.gameObject);
            else
            {
                targetPath = child.gameObject;
            }
        }
        if (EnemySpawner.startedSpawning)
        {
            StartCoroutine(ShootPenny(targetPath));
        }

        

    }

    public IEnumerator ShootPenny(GameObject path){

        projectile = Instantiate(projectilePrefab, Vector3.up * 100, Quaternion.identity);
        projectile.transform.SetParent(transform); //line added by Lupis if it breaks my b
        Debug.Log(path.name);
        attackPath[0] =path.transform;
        
        projectile.GetComponent<FollowPath>().routesAssigned = attackPath ;
        projectile.GetComponent<FollowPath>().isPenny = true;

        projectile.GetComponent<CollisionDamageNoLimit>().towerOwner = GetComponent<TowerController>();
        projectile.GetComponent<CollisionDamageNoLimit>().AttackPower = GetComponent<TowerController>().damage;

        yield break;
   }

    private List<CoffeeBoostScript> coffeeBoosters = new List<CoffeeBoostScript>();
    public void BoostSpeed(CoffeeBoostScript booster)
    {
        Debug.Log("tried boosting");
        if (coffeeBoosters.Contains(booster) || projectile == null)
        {
            return;
        }
        Debug.Log("boosted");
        coffeeBoosters.Add(booster);
        projectile.GetComponent<FollowPath>().speed *= 1.5f;
    }
    public void UnboostSpeed(CoffeeBoostScript booster)
    {
        if (!coffeeBoosters.Contains(booster) || projectile == null)
        {
            return;
        }
        coffeeBoosters.Remove(booster);
        projectile.GetComponent<FollowPath>().speed /= 1.5f;
    }

    void Start()
    {
        foreach(Transform child in this.gameObject.transform)
        {
            PossibleDirections.Add(child.gameObject);
        }

    }

  
    void Update()
    {
        
    }



}
