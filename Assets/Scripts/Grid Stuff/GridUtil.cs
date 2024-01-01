using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Matrix2x2
{
    [SerializeField] private float col0Row0;
    [SerializeField] private float col0Row1;
    [SerializeField] private float col1Row0;
    [SerializeField] private float col1Row1;

    public Matrix2x2(float a, float b, float c, float d)
    {
        col0Row0 = a;
        col1Row0 = b;
        col0Row1 = c;
        col1Row1 = d;
    }

    public void PrintMatrix()
    {
        Debug.Log("\n" + col0Row0 + " " + col1Row0 + "\n" + col0Row1 + " " + col1Row1);
    }

    public Vector2 MultiplyVector(Vector2 vec)
    {
        Vector2 resultVec;
        resultVec.x = col0Row0 * vec.x + col1Row0 * vec.y;
        resultVec.y = col0Row1 * vec.x + col1Row1 * vec.y;
        /*Debug.Log("\n" + resultVec.x + "\n" + resultVec.y);*/
        return resultVec;
    }

    public Matrix2x2 InverseMatrix()
    {
        float determinant = col0Row0 * col1Row1 - col1Row0 * col0Row1;
        if (determinant == 0)
        {
            Debug.Log("This matrix has no inverse");
            return null;
        }
        return new Matrix2x2(col1Row1, col1Row0 * -1, col0Row1 * -1, col0Row0).MultiplyScalar(1 / determinant);
    }

    public Matrix2x2 MultiplyScalar(float scalar)
    {
        return new Matrix2x2(col0Row0 * scalar, col1Row0 * scalar, col0Row1 * scalar, col1Row1 * scalar);
    }
}

public class GridUtil: MonoBehaviour
{
    private int[,] grid;
    private TowerController[,] towerTilePositions;
    [SerializeField] int xTiles;
    [SerializeField] int yTiles;
    [SerializeField] Matrix2x2 transformationMatrix;
    [SerializeField] List<Vector2> roadTilePositions; //Only applies to center
    [SerializeField] GameObject gridVisualizers;

    //The grid positions where the roads turn
    [SerializeField] List<Vector2> road1Turns;
    [SerializeField] List<Vector2> road2Turns;
    [SerializeField] List<Vector2> road3Turns;
    [SerializeField] List<Vector2> road4Turns; //4th road removed
    public Vector2 GridSize
    {
        get
        {
            return new Vector2(xTiles, yTiles);
        }
    }
    public int[,] GridTile
    {
        get
        {
            return grid;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new int[xTiles, yTiles];
        towerTilePositions = new TowerController[xTiles, yTiles];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (roadTilePositions.Contains(new Vector2(x, y)))
                {
                    grid[x, y] = 1;
                }
                /*Instantiate(gridVisualizers, ConvertTileToPosition(x, y), Quaternion.identity);*/
            }
        }
        AddRoadsFromTurns(road1Turns);
        AddRoadsFromTurns(road2Turns);
        AddRoadsFromTurns(road3Turns);
        /*AddRoadsFromTurns(road4Turns);*/
    }

    public Vector3 ConvertTileToPosition(int x, int y)
    {
        return ConvertTileToPosition(new Vector2(x, y));
    }

    public Vector3 ConvertTileToPosition(Vector2 tilePos)
    {
        Vector2 place = transformationMatrix.MultiplyVector(tilePos);
        place.x += transform.position.x;
        place.y += transform.position.y;
        return new Vector3(place.x, place.y, 0);
    }

    public Vector2 ConvertPositionToTile(Vector2 worldPosition)
    {
        Vector2 tileSpot = worldPosition;
        tileSpot.x -= transform.position.x;
        tileSpot.y -= transform.position.y;
        tileSpot = transformationMatrix.InverseMatrix().MultiplyVector(tileSpot);
        /*int horizontalDirection = worldPosition.x < 0 ? -1 : 1;
        int verticalDirection = worldPosition.y < 0 ? -1 : 1;
        Debug.Log(worldPosition.x < 0);
        Debug.Log(horizontalDirection + " " + verticalDirection);*/
        tileSpot.x += 0.5f;
        tileSpot.x = (int)tileSpot.x;
        tileSpot.y += 0.5f;
        tileSpot.y = (int)tileSpot.y;
        return tileSpot;
    }

    //called by TowerMarkerController when a tower is placed
    //adds the tower to the grid so another tower cant be placed on top
    public void AddTowerToGrid(Vector2 tile, Vector2 towerSize, TowerController towerInstance)
    {
        for (int x = (int)tile.x; x < (int)tile.x + towerSize.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + towerSize.y; y++)
            {
                grid[x, y] = 2;
                towerTilePositions[x, y] = towerInstance;
            }
        }
        /*grid[(int)tile.x, (int)tile.y] = 2;*/
    }

    public bool CanPlaceTower(Vector2 tile, Vector2 towerSize)
    {
        bool canPlace = true;
        for (int x = (int)tile.x; x < (int)tile.x + towerSize.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + towerSize.y; y++)
            {
                if (x >= GridSize.x || x < 0 ||
                    y >= GridSize.y || y < 0)
                {
                    canPlace = false;
                    break;
                }
                if (grid[x, y] != 0)
                {
                    canPlace = false;
                    break;
                }
            }
        }
        return canPlace;
    }

    public TowerController GetTowerOnTile(Vector2 tilePosition)
    {
        int x = (int)tilePosition.x;
        int y = (int)tilePosition.y;
        if (x < 0 || x >= GridSize.x || y < 0 || y >= GridSize.y)
        {
            Debug.Log("out of bounds click");
            return null;
        }
        if (grid[x, y] != 2)
        {
            return null;
        }
        return towerTilePositions[x, y];
    }

    //currently unused
    public void RemoveTowerFromGrid(Vector2 tile, Vector2 towerSize)
    {
        for (int x = (int)tile.x; x < (int)tile.x + towerSize.x; x++)
        {
            for (int y = (int)tile.y; y < (int)tile.y + towerSize.y; y++)
            {
                grid[x, y] = 0;
                towerTilePositions[x, y] = null;
            }
        }
        /*grid[(int)tile.x, (int)tile.y] = 0;*/
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(ConvertPositionToTile(mousePos));*/
    }

    //Makes sure you cant place on road tiles based off the turns of each road
    void AddRoadsFromTurns(List<Vector2> roadTurns)
    {
        if (roadTurns.Count < 1)
        {
            return;
        }
        for (int i = 0; i < roadTurns.Count - 1; i++)
        {
            while (Vector2.Distance(roadTurns[i], roadTurns[i + 1]) > 0.1f)
            {
                grid[(int)roadTurns[i].x, (int)roadTurns[i].y] = 1;
                roadTurns[i] = Vector2.MoveTowards(roadTurns[i], roadTurns[i + 1], 1);
            }
        }
        grid[(int)roadTurns[roadTurns.Count - 1].x, (int)roadTurns[roadTurns.Count - 1].y] = 1;
    }
}