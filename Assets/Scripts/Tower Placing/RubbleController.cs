using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubbleController : TowerController
{
    GameObject towerOriginPrefab;
    [SerializeField] GameObject rubbleSegmentPrefab;
    List<GameObject> rubblePieces = new List<GameObject>();
    Vector2 tilePos;
    Vector2 pileSize;
    public RubbleData RubbleInfo
    {
        get
        {
            return new RubbleData(tilePos, pileSize);
        }
    }


    public void InitRubble(Vector2 towerPosition, Vector2 towerSize, GameObject towerOwner)
    {
        //set gridUtil
        gridScript = GameObject.Find("Tower Manager").GetComponent<GridUtil>();
        //set tower manager
        towerManagerScript = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
        //set audio manager
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        //sets the prefab of the tower type that spawned this rubble
        towerOriginPrefab = towerOwner;
        //sets size of rubble pile
        Vector2 tile = gridScript.ConvertPositionToTile(towerPosition);
        //sets these vars for use when destroying
        tilePos = tile;
        pileSize = towerSize;
        //instantiate rubble sprites based off position and size
        for (int x = (int)tile.x; x < (int)tile.x + towerSize.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + towerSize.y; y++)
            {
                GameObject rubblePieceInstance = Instantiate(rubbleSegmentPrefab, gridScript.ConvertTileToPosition(new Vector2(x, y)), Quaternion.identity);
                rubblePieces.Add(rubblePieceInstance);
            }
        }
        //update tower in gridutil grid
        gridScript.RemoveTowerFromGrid(tile, towerSize);
        gridScript.AddTowerToGrid(tile, towerSize, this);
        //update list in tower manager
        towerManagerScript.AddToRubbleList(this);
    }
    public void InitRubble(Vector2 towerPosition, Vector2 towerSize) //overload without tower owner caus lazy
    {
        //set gridUtil
        gridScript = GameObject.Find("Tower Manager").GetComponent<GridUtil>();
        //set tower manager
        towerManagerScript = GameObject.Find("Tower Manager").GetComponent<TowerManager>();
        //set audio manager
        audioManagerScript = GameObject.Find("Audio Manager").GetComponent<AudioManager>();
        //sets size of rubble pile
        Vector2 tile = gridScript.ConvertPositionToTile(towerPosition);
        //sets these vars for use when destroying
        tilePos = tile;
        pileSize = towerSize;
        //instantiate rubble sprites based off position and size
        for (int x = (int)tile.x; x < (int)tile.x + towerSize.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + towerSize.y; y++)
            {
                GameObject rubblePieceInstance = Instantiate(rubbleSegmentPrefab, gridScript.ConvertTileToPosition(new Vector2(x, y)), Quaternion.identity);
                rubblePieces.Add(rubblePieceInstance);
            }
        }
        //update tower in gridutil grid
        gridScript.RemoveTowerFromGrid(tile, towerSize);
        gridScript.AddTowerToGrid(tile, towerSize, this);
        //update list in tower manager
        towerManagerScript.AddToRubbleList(this);
    }

    //called by bottom bar when remove button is pressed
    public void RemoveSelf()
    {
        audioManagerScript.PlaySound("Rubble Remove");
        gridScript.RemoveTowerFromGrid(tilePos, pileSize);
        towerManagerScript.RemoveFromRubbleList(this);
        foreach (GameObject rubblePiece in rubblePieces)
        {
            Destroy(rubblePiece);
        }
        Destroy(gameObject);
    }

    private void Awake()
    {
        //put here to override tower awake
    }

    private void Start()
    {
        //put here to override tower start
    }

    private void Update()
    {
        //put here to override tower update
    }
}
