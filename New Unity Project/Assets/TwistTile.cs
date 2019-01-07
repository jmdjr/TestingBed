using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TwistTile : MonoBehaviour
{
    static Dictionary<Vector3Int, TwistTile> TwistMap = new Dictionary<Vector3Int, TwistTile>();
    [EnumFlags]
    public TapDirections Directions;

    public Vector3Int TilePosition;
    public ITilemap Tilemap;
    public TwistTilePiece TilePiece;

    public Transform TileGraphic;
    public Text PositionText;

    public TwistTile North = null;
    public TwistTile East = null;
    public TwistTile South = null;
    public TwistTile West = null;

    private bool isRotating = false;

    //public void SetDirections(TwistTile north, TwistTile east, TwistTile south, TwistTile west)
    //{
    //    North = north;
    //    East = east;
    //    South = south;
    //    West = west;
    //}

    public void SetParams(Vector3Int pos, ITilemap map, TwistTilePiece piece)
    {
        TilePosition = pos;
        Tilemap = map;
        TilePiece = piece;

        if (TwistMap.ContainsKey(pos))
        {
            TwistMap[pos] = this;
        }
        else
        {
            TwistMap.Add(pos, this);
        }

        //Debug.Log($"params: {pos}");
    }

    //TwistTilePiece tNorth;
    //TwistTilePiece tEast;
    //TwistTilePiece tSouth;
    //TwistTilePiece tWest;

    public IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Vector3Int tempPos = new Vector3Int(1, 1, 0);
        //Vector3Int tempPos = TilePosition;

        //TwistTilePiece tThis = Tilemap.GetTile<TwistTilePiece>(TilePosition);

        TwistTilePiece tThis = TilePiece;

        //TwistTilePiece tNorth = tThis.North;
        //TwistTilePiece tEast = tThis.East;
        //TwistTilePiece tSouth = tThis.South;
        //TwistTilePiece tWest = tThis.West;

        //TwistTilePiece tNorth = Tilemap.GetTile<TwistTilePiece>(TilePosition);
        //TwistTilePiece tEast = Tilemap.GetTile<TwistTilePiece>(TilePosition);
        //TwistTilePiece tSouth = Tilemap.GetTile<TwistTilePiece>(TilePosition);
        //TwistTilePiece tWest = Tilemap.GetTile<TwistTilePiece>(TilePosition);

        //TileData tNorth = new TileData(), tEast = new TileData(), tSouth = new TileData(), tWest = new TileData();

        //TilePiece.GetTileData(TilePosition, Tilemap, ref tNorth);
        //TilePiece.GetTileData(TilePosition, Tilemap, ref tEast);
        //TilePiece.GetTileData(TilePosition, Tilemap, ref tSouth);
        //TilePiece.GetTileData(TilePosition, Tilemap, ref tWest);

        //SetDirections(
        //    tNorth.<TwistTile>(),
        //    tEast.gameObject.GetComponent<TwistTile>(),
        //    tSouth.gameObject.GetComponent<TwistTile>(),
        //    tWest.gameObject.GetComponent<TwistTile>()
        //    );

        //SetDirections(
        //    tNorth == null ? null : tNorth.myTileObject,
        //    tEast == null ? null : tEast.myTileObject,
        //    tSouth == null ? null : tSouth.myTileObject,
        //    tWest == null ? null : tWest.myTileObject);

        var tempKey = TilePosition + Vector3Int.up;
        North = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.right;
        East = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.down;
        South = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.left;
        West = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        //string me = $"({TilePosition.x}, {TilePosition.y})";

        //string north = North ? $"{North.TilePosition.x}, {North.TilePosition.y}" : "null";
        //string east = East ? $"{East.TilePosition.x}, {East.TilePosition.y}" : "null";
        //string south = South ? $"{South.TilePosition.x}, {South.TilePosition.y})" : "null";
        //string west = West ? $"{West.TilePosition.x}, {West.TilePosition.y}" : "null";

        //string north = tNorth ? $"{tNorth.myPosition.x}, {tNorth.myPosition.y}" : "null";
        //string east = tEast ? $"{tEast.myPosition.x}, {tEast.myPosition.y}" : "null";
        //string south = tSouth ? $"{tSouth.myPosition.x}, {tSouth.myPosition.y})" : "null";
        //string west = tWest ? $"{tWest.myPosition.x}, {tWest.myPosition.y}" : "null";

        //if (PositionText)
        //{
        //    PositionText.text = ""; // $"{north}\n\n{west}{me}{east}\n\n{south}";
        //    //Debug.Log($"fromPiece: {tThis.myPosition} | {me}\n\n{PositionText.text}");
        //}
    }

    public void Rotate(int dir)
    {
        if (!isRotating && dir != 0)
        {
            TapDirections tapDir = Directions;

            if (dir > 0)
            {
                Directions = Directions.RotateClockwise();
                StartCoroutine(RotationAnimation(-90));
            }
            else
            {
                Directions = Directions.RotateCounterClockwise();
                StartCoroutine(RotationAnimation(90));
            }

            isRotating = true;
        }
    }

    public bool Has(TapDirections dir)
    {
        return Directions.HasFlag(dir);
    }

    public void OnMouseDown()
    {
        Rotate(1);
    }

    public void OnMouseOver()
    {
        //Debug.Log(Directions.ToString());
    }

    private IEnumerator RotationAnimation(int degrees)
    {
        FloatTween tweener = TweenFactory.Tween(null, 0, degrees, 0.3f, TweenScaleFunctions.CubicEaseInOut, null);

        float currentRotationZ = TileGraphic.eulerAngles.z;

        while (tweener.State == TweenState.Running)
        {
            TileGraphic.eulerAngles = new Vector3(0, 0, currentRotationZ + tweener.CurrentValue);
            yield return new WaitForEndOfFrame();
        }

        TileGraphic.eulerAngles = new Vector3(0, 0, currentRotationZ + degrees);

        TriggerCascade(-1 * Math.Sign(degrees));

        isRotating = false;

        yield return 0;
    }

    private void TriggerCascade(int dir)
    {
        if (Has(TapDirections.East) && East && East.Has(TapDirections.West)) { East.Rotate(dir); }
        if (Has(TapDirections.North) && North && North.Has(TapDirections.South)) { North.Rotate(dir); }
        if (Has(TapDirections.West) && West && West.Has(TapDirections.East)) { West.Rotate(dir); }
        if (Has(TapDirections.South) && South && South.Has(TapDirections.North)) { South.Rotate(dir); }
    }
}
