using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_Randomizer : MonoBehaviour
{
    private List<List<Vector2Int>> IslandGroups;

    public int Height = 25;
    public int Width = 25;

    [Range(0, 1)]
    public float randomChance = 0.45f;
    public int MaxIslandCount = 5;

    [Header("Simulation Stepper")]
    public int DeathLimit = 3;
    public int BirthLimit = 2;
    [Range(0, 1)]
    public float randomIslandChance = 0.5f;

    public int Iterations = 10;
    public float TimeDelay = 3;

    public Tilemap TileMap;
    public Tile BaseTile;

    private int halfWidth, halfHeight;
    private bool[,] OldMap;

    // Start is called before the first frame update
    void Start()
    {
        halfWidth = Width / 2;
        halfHeight = Height / 2;

        IslandGroups = new List<List<Vector2Int>>();

        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        ClearOldGeneratedMap();

        initializeMapOfLand();

        IterateOldMap();

        GenerateMapOfLandTiles();

        yield return new WaitForSeconds(TimeDelay);

        StartCoroutine(IslandMethodRoutine());
    }


    private IEnumerator IslandMethodRoutine()
    {
        IslandGroups.Clear();

        ClearOldGeneratedMap();

        initializeMapOfLand_IslandCount();

        for (int i = 0; i < Iterations; ++i)
        {
            bool[,] newMap = SimulateMapStep_IslandCount();

            if (OldMap != newMap)
            {
                ClearOldGeneratedMap();
                OldMap = newMap;
                GenerateMapOfLandTiles();
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                break;
            }
        }

        yield return new WaitForSeconds(TimeDelay);

        StartCoroutine(IslandMethodRoutine());
    }



    private void ClearOldGeneratedMap()
    {
        TileMap.ClearAllTiles();
        TileController.ClearReferencesMap();
    }

    private void IterateOldMap()
    {
        for (int i = 0; i < Iterations; ++i)
        {
            bool[,] newMap = SimulateMapStep();

            if (OldMap != newMap)
            {
                OldMap = newMap;
            }
            else
            {
                break;
            }
        }
    }
    
    private void IterateOldMap_IslandCount()
    {
        for (int i = 0; i < Iterations; ++i)
        {
            bool[,] newMap = SimulateMapStep_IslandCount();

            if (OldMap != newMap)
            {
                OldMap = newMap;
            }
            else
            {
                break;
            }
        }
    }

    private void initializeMapOfLand()
    {
        OldMap = new bool[Width, Height];
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                OldMap[x, y] = SLUtility.Random.Chance(randomChance);
            }
        }
    }
    
    private void initializeMapOfLand_IslandCount()
    {
        OldMap = new bool[Width, Height];

        int islandCount = 0;
        int halfHalfWidth = halfWidth / 2;
        int halfHalfHeight = halfHeight / 2;

        for (int x = halfHalfWidth; x < (Width); ++x)
        {
            for (int y = halfHalfHeight; y < (Height); ++y)
            {
                if(islandCount < MaxIslandCount && SLUtility.Random.Chance(randomChance))
                {
                    OldMap[x, y] = true;

                    List<Vector2Int> islandList = new List<Vector2Int>();
                    islandList.Add(new Vector2Int(x, y));
                    IslandGroups.Add(islandList);

                    islandCount += 1;
                }
                else
                {
                    OldMap[x, y] = false;
                }

            }
        }
    }

    private bool[,] SimulateMapStep()
    {
        bool[,] newMap = new bool[Width, Height];

        for (int a = 0; a < Width; ++a)
        {
            for (int b = 0; b < Height; ++b)
            {
                int liveNeighbors = AliveNeighboursCount(a, b);

                if (OldMap[a, b])
                {
                    newMap[a, b] = liveNeighbors >= BirthLimit;
                }
                else
                {
                    newMap[a, b] = liveNeighbors <= DeathLimit;
                }
            }
        }

        return newMap;
    }

    private bool[,] SimulateMapStep_IslandCount()
    {
        bool[,] newMap = new bool[Width, Height];

        for (int a = 0; a < IslandGroups.Count; ++a)
        {
            List<Vector2Int> island = IslandGroups[a];
            List<Vector2Int> newPositions = new List<Vector2Int>();

            for (int b = 0; b < island.Count; ++b)
            {
                Vector2Int islandChunk = island[b];
                newMap[islandChunk.x, islandChunk.y] = true;

                List<Vector2Int> available = AliveNeighbours_Sides(islandChunk.x, islandChunk.y);

                if(available.Count >= 2)
                {
                    Vector2Int newChunk = available.RandomOne();

                    if (!PointOnOtherIslands(new Vector2Int(newChunk.x, newChunk.y), a))
                    {
                        newPositions.Add(newChunk);
                        newMap[newChunk.x, newChunk.y] = true;
                    }
                }
            }

            IslandGroups[a].AddRange(newPositions);
        }

        return newMap;
    }

    private bool PointOnOtherIslands(Vector2Int point, int islandIndex)
    {
        bool found = false;
        for(int a = 0; a < IslandGroups.Count; ++a)
        {
            if(IslandGroups[a] != null)
            {
                found = found || IslandGroups[a].Any(p => p == point);
            }
        }

        return found;
    }

    private void GenerateMapOfLandTiles()
    {
        for (int x = 0; x < Width; ++x)
        {
            for (int y = 0; y < Height; ++y)
            {
                if (OldMap[x, y])
                {
                    TileMap.SetTile(new Vector3Int(x - halfWidth, y - halfHeight, 0), BaseTile);
                }
            }
        }
    }

    // running around the perimeter
    private IEnumerator<Vector2Int> AliveNeighbours_IslandCount(int x, int y)
    {
        //string row1 = "", row2 = "", row3 = "";

        bool isEven = (y % 2) == 0;

        for (int nx = isEven ? -1 : 0, maxX = isEven ? 0 : 1; nx < maxX; ++nx)
        {
            for (int ny = -1; ny < 2; ++ny)
            {
                int n_x = nx + x, n_y = ny + y;

                // are we at the center
                if (ny == 0 && nx == 0)
                {
                    n_x += (isEven ? 1 : -1);
                }

                // are we on the border of the map.
                if (n_x < 0 || n_x >= Width || n_y < 0 || n_y >= Height)
                {
                    // count it for a cave effect, tweak as necessary.
                    //count += 1;
                }
                // well, we are fine then
                else if (!OldMap[n_x, n_y])
                {
                    yield return new Vector2Int(n_x, n_y);
                }
            }
        }

        //yield return Vector2Int.zero;
    }

    private List<Vector2Int> AliveNeighbours_Sides(int x, int y)
    {
        List<Vector2Int> sidePoints = new List<Vector2Int>();
        bool isEven = (y % 2) == 0;

        for (int nx = isEven ? -1 : 0, maxX = isEven ? 0 : 1; nx <= maxX; ++nx)
        {
            for (int ny = -1; ny < 2; ++ny)
            {
                int n_x = nx + x, n_y = ny + y;

                // are we at the center
                if (ny == 0 && nx == 0)
                {
                    n_x += (isEven ? 1 : -1);
                }

                // are we on the border of the map.
                if (n_x < 0 || n_x >= Width || n_y < 0 || n_y >= Height)
                {
                    // count it for a cave effect, tweak as necessary.
                    //count += 1;
                }
                else
                {
                    sidePoints.Add(new Vector2Int(n_x, n_y));
                }
            }
        }

        return sidePoints;
    }


    // running around the perimeter
    private int AliveNeighboursCount(int x, int y)
    {
        int count = 0;
        //string row1 = "", row2 = "", row3 = "";
        bool isEven = (y % 2) == 0;

        for (int nx = isEven ? -1 : 0, maxX = isEven ? 0 : 1; nx < maxX; ++nx)
        {
            for(int ny = -1; ny < 2; ++ny)
            {
                int n_x = nx + x, n_y = ny + y;

                // are we at the center
                if (ny == 0 && nx == 0)
                {
                    n_x += (isEven ? 1 : -1);
                }

                // are we on the border of the map.
                if (n_x < 0 || n_x >= Width || n_y < 0 || n_y >= Height)
                {
                    // count it for a cave effect, tweak as necessary.
                    //count += 1;
                }
                // well, we are fine then
                else if(OldMap[n_x, n_y])
                {
                    count += 1;
                }

                //string point = $"{n_x}, {n_y}";

                //switch (ny)
                //{
                //    case -1:
                //        row1 += point;
                //        break;
                //    case 0:
                //        row2 += point;
                //        break;
                //    case 1:
                //        row3 += point;
                //        break;

                //}
            }
        }

        //Debug.Log($"{row1}\n{row2}\n{row3}");

        return count;
    }


}
