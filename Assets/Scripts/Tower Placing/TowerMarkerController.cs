using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerMarkerController : MonoBehaviour
{
    Camera mainCam;
    [HideInInspector] public GameObject correspondingTowerPrefab;
    private Transform attackRadiusIndicator;
    private Vector3 mousePos;
    private Action movementMethod;

    private GridUtil gridScript;
    private bool inGrid;
    private Core coreScript;
    private BottomBarController bottomBarScript;
    private FirstLevelManager sceneManager;

    private void Awake()
    {
        DataStore.enablePointAndClickTower += ChangeMovementMethod;
    }

    private void OnDestroy()
    {
        DataStore.enablePointAndClickTower -= ChangeMovementMethod;
    }

    private void Start()
    {
        /*placeCheckRadius = correspondingTowerPrefab.GetComponent<CircleCollider2D>().radius;*/
        gridScript = GameObject.Find("Tower Manager").GetComponent<GridUtil>();
        coreScript = GameObject.Find("Core").GetComponent<Core>();
        bottomBarScript = GameObject.Find("Canvas").transform.Find("Bottom Bar").GetComponent<BottomBarController>();
        sceneManager = GameObject.Find("Scene Manager").GetComponent<FirstLevelManager>();
        mainCam = Camera.main;
        GetComponent<SpriteRenderer>().sprite = correspondingTowerPrefab.GetComponent<SpriteRenderer>().sprite;
        attackRadiusIndicator = transform.Find("Tower Radius Visual");
        float attackRadius = correspondingTowerPrefab.GetComponent<TowerController>().AttackRadius;
        attackRadiusIndicator.transform.localScale = new Vector3(attackRadius * 2, attackRadius * 2, 1);
        attackRadiusIndicator.transform.localPosition = correspondingTowerPrefab.GetComponent<TowerController>().TowerCenterTransform.position;
        attackRadiusIndicator.gameObject.SetActive(true);
        StartCoroutine(CheckIfOnGrid());
        ChangeMovementMethod();
    }

    private void Update()
    {
        movementMethod();
    }

    private void ChangeMovementMethod()
    {
        if (DataStore.pointAndClickTowerEnabled)
        {
            movementMethod = ClickTowerMarker;
        }
        else
        {
            movementMethod = DragTowerMarker;
        }
    }
    //Methods for moving the marker
    private void DragTowerMarker()
    {
        if (Input.GetMouseButton(0))
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (inGrid)
            {
                transform.position = gridScript.ConvertTileToPosition(gridScript.ConvertPositionToTile(mousePos));
            }
            else
            {
                transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            }
            if (CanPlace())
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            }
        }
        else
        {
            TryToPlaceTowerAtMousePos();
        }
    }
    private void ClickTowerMarker()
    {
        //this gameobject should follow cursor unconditionally
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (inGrid)
        {
            transform.position = gridScript.ConvertTileToPosition(gridScript.ConvertPositionToTile(mousePos));
        }
        else
        {
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
        if (CanPlace())
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
        }
        //try to place tower when left click is pressed
        if (Input.GetMouseButtonDown(0))
        {
            TryToPlaceTowerAtMousePos();
        }
    }
    private void TryToPlaceTowerAtMousePos()
    {
        if (CanPlace() && coreScript.CanAfford(correspondingTowerPrefab.GetComponent<TowerController>().Price))
        {
            coreScript.SpendEnergy(correspondingTowerPrefab.GetComponent<TowerController>().Price);
            Vector2 mouseTile = gridScript.ConvertPositionToTile(mousePos);
            //next two lines actually place the tower
            TowerController towerInstance = Instantiate(correspondingTowerPrefab, transform.position, Quaternion.identity).GetComponent<TowerController>();
            gridScript.AddTowerToGrid(mouseTile, towerInstance.Size, towerInstance);
            bottomBarScript.SwitchToTowerDisplay(towerInstance.gameObject);
            if (FirstLevelManager.tutorialActive)
            {
                sceneManager.TowerPlacedResponse();
            }
        }
        //destroys tower marker so player can move their cursor normally again
        Destroy(gameObject);
    }

    private bool CanPlace()
    {
        if (FirstLevelManager.tutorialActive)
        {
            return TutorialCanPlace();
        }
        Vector2 currentTile = gridScript.ConvertPositionToTile(mousePos);
        return !bottomBarScript.IsMouseOnBar() &&
            gridScript.CanPlaceTower(currentTile, correspondingTowerPrefab.GetComponent<TowerController>().Size);
    }
    private bool TutorialCanPlace()
    {
        return gridScript.ConvertPositionToTile(mousePos) == new Vector2(11, 4);
    }

    IEnumerator CheckIfOnGrid()
    {
        while (true)
        {
            Vector2 currentTile = gridScript.ConvertPositionToTile(mousePos);
            Vector2 gridSize = gridScript.GridSize;
            if (currentTile.x > gridSize.x - 1 || currentTile.x < 0
                || currentTile.y > gridSize.y - 1 || currentTile.y < 0)
            {
                inGrid = false;
            }
            else
            {
                inGrid = true;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
