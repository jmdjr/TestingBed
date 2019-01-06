using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TwistTile : MonoBehaviour, IInteractable
{
    [EnumFlags]
    public TapDirections Directions;

    public Transform TileGraphic;

    public TwistTile North;
    public TwistTile East;
    public TwistTile South;
    public TwistTile West;

    public void setDirections(TwistTile north, TwistTile east, TwistTile south, TwistTile west)
    {
        North = north;
        East = east;
        South = south;
        West = west;
    }

    public void Interact()
    {

    }

    public void OnMouseDown()
    {

    }

    public void OnMouseOver()
    {
        Debug.Log(Directions.ToString());
    }
}
