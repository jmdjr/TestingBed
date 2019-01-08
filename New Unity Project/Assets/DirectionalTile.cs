using DigitalRuby.Tween;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DirectionalTile : MonoBehaviour
{
    static Dictionary<Vector3Int, DirectionalTile> MyMap = new Dictionary<Vector3Int, DirectionalTile>();

    [EnumFlags]
    [SerializeField]
    private TapDirections Directions;

    public Vector3Int Position { get; private set; }

    [SerializeField]
    private Transform TileGraphic;

    [SerializeField]
    private Text PositionText;

    public DirectionalTile North { get; private set; }
    public DirectionalTile East { get; private set; }
    public DirectionalTile South { get; private set; }
    public DirectionalTile West { get; private set; }

    public void SetParams(Vector3Int pos, ITilemap map, TwistTilePiece piece)
    {
        if (MyMap.ContainsKey(pos))
        {
            MyMap[pos] = this;
        }
        else
        {
            MyMap.Add(pos, this);
        }
        
        Position = pos;
    }
    public IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();

        Vector3Int tempKey = Position + Vector3Int.up;
        North = MyMap.ContainsKey(tempKey) ? MyMap[tempKey] : null;

        tempKey = Position + Vector3Int.right;
        East = MyMap.ContainsKey(tempKey) ? MyMap[tempKey] : null;

        tempKey = Position + Vector3Int.down;
        South = MyMap.ContainsKey(tempKey) ? MyMap[tempKey] : null;

        tempKey = Position + Vector3Int.left;
        West = MyMap.ContainsKey(tempKey) ? MyMap[tempKey] : null;
    }

    public bool HasDirection(TapDirections dir)
    {
        return Directions.HasFlag(dir);
    }

    private bool isRotating = false;
    private bool isDragging = false;
    private int ActiveDirection = 0;
    private Vector3 DownSpot = new Vector3();

    public void OnMouseDown()
    {
        DownSpot = Input.mousePosition;
    }

    public void OnMouseDrag()
    {
        var dir = Input.mousePosition - DownSpot;
        float threshhold = 10;

        if(dir.magnitude != 0 && dir.magnitude > threshhold)
        {
            isDragging = true;
            ActiveDirection = Math.Sign(dir.x);
        }
    }

    public void OnMouseUp()
    {
        if(isDragging)
        {
            Rotate(ActiveDirection);
        }
    }

    public void Rotate(int dir)
    {
        if (!isRotating && dir != 0)
        {
            TapDirections tapDir = Directions;

            if (dir > 0)
            {
                Directions = Directions.RotateClockwise(2);
                StartCoroutine(RotationAnimation(-90));
            }
            else
            {
                Directions = Directions.RotateCounterClockwise(2);
                StartCoroutine(RotationAnimation(90));
            }

            isRotating = true;
        }
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

        TriggerCascade(Math.Sign(degrees));

        isRotating = false;
        ActiveDirection = 0;
        yield return 0;
    }

    private void TriggerCascade(int dir)
    {
        if (HasDirection(TapDirections.East) && East && East.HasDirection(TapDirections.West)) { East.Rotate(dir); }
        if (HasDirection(TapDirections.North) && North && North.HasDirection(TapDirections.South)) { North.Rotate(dir); }
        if (HasDirection(TapDirections.West) && West && West.HasDirection(TapDirections.East)) { West.Rotate(dir); }
        if (HasDirection(TapDirections.South) && South && South.HasDirection(TapDirections.North)) { South.Rotate(dir); }


    }
}
