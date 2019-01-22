using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_Randomizer : MonoBehaviour
{
    public int Height = 13;
    public int Width = 11;

    [Range(0, 1)]
    public float randomChance = 0.45f;
    public int MinIslandCount = 5;


    [Header("Simulation Stepper")]
    public int DeathLimit = 3;
    public int BirthLimit = 2;

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

        initializeMapOfLand();

        IterateOldMap();

        GenerateMapOfLandTiles();

        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        yield return new WaitForSeconds(TimeDelay);

        ClearOldMap();

        initializeMapOfLand();

        IterateOldMap();

        GenerateMapOfLandTiles();

        StartCoroutine(Routine());
    }

    private void ClearOldMap()
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

    private bool[,] SimulateMapStep()
    {
        bool[,] newMap = new bool[Width, Height];

        for (int a = 0; a < Width; ++a)
        {
            for (int b = 0; b < Height; ++b)
            {
                int liveNeighbors = CountAliveNeighbours(a, b);
                
                if(OldMap[a, b])
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
    private int CountAliveNeighbours(int x, int y)
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
