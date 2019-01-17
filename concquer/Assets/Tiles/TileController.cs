﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    static Dictionary<Vector3Int, TileController> HexTileMap = new Dictionary<Vector3Int, TileController>();

    Vector3Int MyTilePosition;
    ITilemap MyTileMap;
    TileControllerPiece MyTilePiece;

    TileController[] neighborTiles = new TileController[6];

    PawnDataTypes PawnTileDictionary = null;

    [SerializeField]
    PawnType MyTilePawn = PawnType.NONE;

    [SerializeField]
    StructureType MyTileBuilding = StructureType.NONE;

    public void SetParams(Vector3Int position, ITilemap tilemap, TileControllerPiece piece)
    {
        MyTilePosition = position;
        MyTileMap = tilemap;
        MyTilePiece = piece;

        if (HexTileMap.ContainsKey(position))
        {
            HexTileMap[position] = this;
        }
        else
        {
            HexTileMap.Add(position, this);
        }
    }

    public void Start()
    {
        MapNeighbors();
    }

    private void MapNeighbors()
    {
        Vector3Int tempKey = Vector3Int.zero;

        tempKey = MyTilePosition + Vector3Int.up;
        neighborTiles[0] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        tempKey = MyTilePosition + Vector3Int.right;
        neighborTiles[1] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        tempKey = MyTilePosition + Vector3Int.down;
        neighborTiles[2] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        tempKey = MyTilePosition + Vector3Int.down + Vector3Int.left;
        neighborTiles[3] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        tempKey = MyTilePosition + Vector3Int.left;
        neighborTiles[4] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        tempKey = MyTilePosition + Vector3Int.up + Vector3Int.left;
        neighborTiles[5] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;
    }

    public bool HasPawn()
    {
        return MyTilePawn != PawnType.NONE;
    }

    public bool HasBuilding()
    {
        return MyTileBuilding != StructureType.NONE;
    }

    public bool CanPlacePiece(PawnType pawn)
    {
        return (!HasBuilding() || pawn.CanBeat(MyTileBuilding)) && (!HasPawn() || pawn.CanBeat(MyTilePawn));
    }
    //public bool CanPlacePiece(StructureType structure)
    //{
    //    return !HasBuilding() && (!HasPawn() || MyTile)
    //}
}
