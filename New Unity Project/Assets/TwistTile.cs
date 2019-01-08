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
    private TwistTilePiece TilePiece;

    public Transform TileGraphic;
    public Text PositionText;

    private TwistTile North = null;
    private TwistTile East = null;
    private TwistTile South = null;
    private TwistTile West = null;

    private bool isRotating = false;
    private bool isDragging = false;
    private int ActiveDirection = 0;

    public void SetParams(Vector3Int pos, ITilemap map, TwistTilePiece piece)
    {
        if (TwistMap.ContainsKey(pos))
        {
            TwistMap[pos] = this;
        }
        else
        {
            TwistMap.Add(pos, this);
        }

        TilePosition = pos;
    }

    public IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Vector3Int tempPos = new Vector3Int(1, 1, 0);

        TwistTilePiece tThis = TilePiece;

        var tempKey = TilePosition + Vector3Int.up;
        North = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.right;
        East = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.down;
        South = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;

        tempKey = TilePosition + Vector3Int.left;
        West = TwistMap.ContainsKey(tempKey) ? TwistMap[tempKey] : null;
    }

    public bool Has(TapDirections dir)
    {
        return Directions.HasFlag(dir);
    }

    private Vector3 DownSpot = new Vector3();

    public void OnMouseDown()
    {
        DownSpot = Input.mousePosition;
    }

    public void OnMouseDrag()
    {
        var dir = Input.mousePosition - DownSpot;
        float threshhold = 50;

        //Debug.Log($"dragMag: {dir.magnitude} | Direction: {Math.Sign(dir.x)}");

        if (dir.magnitude != 0 && dir.magnitude > threshhold)
        {
            isDragging = true;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Debug.Log($"Angle: {angle}");
            ActiveDirection =  Math.Sign(dir.x);
        }
    }

    public void OnMouseUp()
    {
        if(isDragging)
        {
            Rotate(ActiveDirection);
            isDragging = false;
        }
    }

    public void Rotate(int dir)
    {
        var steps = 2;
        var degrees = 45;
        if (!isRotating && dir != 0)
        {
            TapDirections tapDir = Directions;

            if (dir > 0)
            {
                Directions = Directions.RotateClockwise(steps);
                StartCoroutine(RotationAnimation(-1 * steps * degrees));
            }
            else
            {
                Directions = Directions.RotateCounterClockwise(steps);
                StartCoroutine(RotationAnimation(steps * degrees));
            }

            isRotating = true;
        }
    }

    private void FixedUpdate()
    {
        
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

        isRotating = false;

        TriggerCascade(Math.Sign(degrees));

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
