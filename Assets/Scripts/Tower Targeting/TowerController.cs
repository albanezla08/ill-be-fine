using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    protected TowerManager towerManagerScript;
    protected GridUtil gridScript;
    protected AudioManager audioManagerScript;
    public TowerManager TMScript
    {
        get { return towerManagerScript; }
    }
    public GameObject towerMarker; //i dont think this is actually used anymore
    public GameObject towerPrefab;
    private GameObject radiusIndicator;
    private Animator ownAnimator;
    private SpriteRenderer ownSpriteRenderer;

    [SerializeField] private string towerName;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite clickBoxSprite;
    [SerializeField] [TextArea(3, 10)] private string towerDescription;
    [SerializeField] private float attackRadius = 20;
    [SerializeField] private int price = 300;
    [SerializeField] private int upkeepPrice = 100;
    [SerializeField] private int sellValue = 150;
    [SerializeField] public int damage = 1;
    [SerializeField] private Vector2 size = new Vector2(1, 1);
    private Vector2 tilePosition;
    public string TowerName { get { return towerName; } }
    public string TowerDescription { get { return towerDescription; } }
    public int Price {get {return price;} }
    public int UpkeepPrice { get { return upkeepPrice; } }
    public int SellValue { get { return sellValue; } }
    public Vector2 Size { get { return size; } }
    public Transform TowerCenterTransform { get { return towerCenterTransform; } }
    public float AttackRadius { get { return attackRadius; } }
    [SerializeField] private TowerAttackType attackScript;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform towerCenterTransform;
    private Vector2 towerCenterVec;

    private RaycastHit2D[] targetsArray;
    private List<GameObject> targetsList = new List<GameObject>();
    private List<move> additionalTargetsFromProjectiles = new List<move>();
    private GameObject currentTargetObject;
    private string currentTargetID;

    //upkeep stuff
    [SerializeField] private GameObject rubblePrefab;
    [SerializeField] float timeToRubble;
    float rubbleTimer;
    public float timeTillRubble
    {
        get
        {
            return (int)(rubbleTimer * 100) / 100f;
        }
    }
    public float maxRubbleTime
    {
        get { return (int)(timeToRubble * 100) / 100f; }
    }

    private void OnDestroy()
    {
        DataStore.enableTowerClickboxes -= SwitchSprite;
    }

    private void Awake()
    {
        DataStore.enableTowerClickboxes += SwitchSprite;
        towerCenterVec = towerCenterTransform.position;
        radiusIndicator = transform.Find("Tower Selected Visual").gameObject;
        radiusIndicator.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);
    }

    private void Start()
    {
        towerManagerScript = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        if (GetComponent<PennyRoller>() != null)
        {
            GetComponent<PennyRoller>().towerManager = towerManagerScript;
        }
        gridScript = towerManagerScript.GetComponent<GridUtil>();
        towerManagerScript.AddToTowersList(this);
        ownAnimator = GetComponent<Animator>();
        ownSpriteRenderer = GetComponent<SpriteRenderer>();
        rubbleTimer = timeToRubble;
        if (DataStore.towerClickboxesEnabled)
        {
            ownSpriteRenderer.sprite = clickBoxSprite;
        }
        audioManagerScript.PlaySound("Tower Place");
    }

    private void Update()
    {
        if (!EnemySpawner.startedSpawning)
        {
            return;
        }
        rubbleTimer -= Time.deltaTime;
        if (rubbleTimer <= 10)
        {
            ownAnimator.SetBool("tenSecondsLeft", true);
            if (rubbleTimer <= 0)
            {
                ownAnimator.SetBool("tenSecondsLeft", false);
                DecayTower();
            }
        }
        else
        {
            ownAnimator.SetBool("tenSecondsLeft", false);
        }
    }

    //called by towerMarker on spawn
    public void SetTilePosition(Vector2 pos)
    {
        tilePosition = pos;
    }

    public void UpdateTargetsList()
    {
        targetsArray = Physics2D.CircleCastAll(towerCenterVec, attackRadius, Vector2.zero, attackRadius, targetLayer);
        /*Debug.Log("Current ID: " + currentTargetID);*/
        //remove enemies that left the radius from the list
        if (currentTargetID != null)
        {
            List<GameObject> enemiesToRemove = new List<GameObject>();
            foreach (GameObject enemy in targetsList)
            {
                bool isEnemyGone = true;
                if (enemy != null)
                {
                    foreach (RaycastHit2D target in targetsArray)
                    {
                        if (target.transform.GetComponent<move>().see == enemy.GetComponent<move>().see)
                        {
                            isEnemyGone = false;
                        }
                    }
                }
                if (isEnemyGone)
                {
                    enemiesToRemove.Add(enemy);
                }
            }
            foreach (GameObject enemyToRemove in enemiesToRemove)
            {
                targetsList.Remove(enemyToRemove);
            }
        }
        //add new enemies to the list
        foreach (RaycastHit2D target in targetsArray)
        {
            if (!targetsList.Contains(target.transform.gameObject))
            {
                targetsList.Add(target.transform.gameObject);
            }
        }
    }

    public void PickTarget()
    {
        if (targetsList.Count > 0)
        {
            currentTargetObject = targetsList[0];
            currentTargetID = currentTargetObject.GetComponent<move>().see;
            towerManagerScript.AddToEnemiesList(currentTargetObject.GetComponent<move>());
        }
        else
        {
            currentTargetObject = null;
            currentTargetID = null;
        }
        foreach (move additionalTarget in additionalTargetsFromProjectiles)
        {
            towerManagerScript.AddToEnemiesList(additionalTarget);
        }
        additionalTargetsFromProjectiles.Clear();
    }

    public void AddTargetsFromProjectile(move additionalTarget)
    {
        additionalTargetsFromProjectiles.Add(additionalTarget);
    }

    public void AttackTarget()
    {
        if (currentTargetID != null)
        {
            /*Debug.Log("Attacking " + currentTargetID);*/
            attackScript.towerOwner = this;
            attackScript.TryToAttack(currentTargetObject);
        }
    }

    /*private void OnMouseDown()
    {
        Debug.Log("Pressed");
        towerManagerScript.ChangeBarDisplay(gameObject);
        radiusIndicator.SetActive(true);
    }*/

    public void TowerPressed()
    {
        Debug.Log("Pressed");
        radiusIndicator.SetActive(true);
    }

    //called by Bottom bar controller when the player clicks off the tower
    public void TowerDeselected()
    {
        radiusIndicator.SetActive(false);
    }

    //called by bottombar when upkeep button is pressed
    public void UpkeepTower()
    {
        /*rubbleTimer = timeToRubble;*/
        audioManagerScript.PlaySound("Tower Upkeep");
        if (rubbleTimer + 10 > timeToRubble)
        {
            rubbleTimer = timeToRubble;
        }
        else
        {
            rubbleTimer += 10;
        }
    }

    //called by bottombar when sell button is pressed
    public void SellTower()
    {
        DecayTower();
        /*gridScript.RemoveTowerFromGrid(gridScript.ConvertPositionToTile(transform.position), size);
        towerManagerScript.QueueTowerDestroy(this);*/
    }

    private void DecayTower()
    {
        /*Vector2 tile = gridScript.ConvertPositionToTile(transform.position);
        for (int x = (int)tile.x; x < (int)tile.x + size.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + size.y; y++)
            {
                GameObject rubbleInstance = Instantiate(rubblePrefab, gridScript.ConvertTileToPosition(new Vector2(x, y)), Quaternion.identity);
            }
        }*/
        audioManagerScript.PlaySound("Tower Decay");
        GameObject rubbleInstance = Instantiate(rubblePrefab);
        rubbleInstance.GetComponent<RubbleController>().InitRubble(transform.position, size, gameObject);
        towerManagerScript.QueueTowerDestroy(this);
    }

    //tutorial purposes only
    public void RemoveTowerNoRubble()
    {
        gridScript.RemoveTowerFromGrid(gridScript.ConvertPositionToTile(transform.position), size);
        towerManagerScript.QueueTowerDestroy(this);
    }
    public void PauseRubbleTimer()
    {
        StartCoroutine(FloorRubbleTimer());
    }
    IEnumerator FloorRubbleTimer()
    {
        while (true)
        {
            if (rubbleTimer < 5)
            {
                rubbleTimer = 5;
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    //called by DataStore event
    private void SwitchSprite()
    {
        if (DataStore.towerClickboxesEnabled)
        {
            ownSpriteRenderer.sprite = clickBoxSprite;
        }
        else
        {
            ownSpriteRenderer.sprite = defaultSprite;
        }
    }
}
