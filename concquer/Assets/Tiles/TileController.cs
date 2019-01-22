using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    static Dictionary<Vector3Int, TileController> HexTileMap = new Dictionary<Vector3Int, TileController>();

    Vector3Int MyTilePosition;
    ITilemap MyTileMap;
    TileControllerPiece MyTilePiece;

    public static void ClearReferencesMap()
    {
        HexTileMap.Clear();
    }

    TileController[] neighborTiles = new TileController[HexTileDirection.Size.Id()];

    [SerializeField]
    PawnDataTypes PawnTileDictionary = null;

    [SerializeField]
    SpriteRenderer PieceRenderer = null;

    [SerializeField]
    SpriteRenderer TileRenderer = null;

    [SerializeField]
    PawnType MyTilePawn = PawnType.NONE;

    [SerializeField]
    StructureType MyTileBuilding = StructureType.NONE;

    [Header("Debug Data")]
    public TextMeshPro[] Displays = new TextMeshPro[HexTileDirection.Size.Id()];

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

    public Vector3Int GetTilePosition()
    {
        return MyTilePosition;
    }

    public void Start()
    {
        MapNeighbors();
    }

    private void MapNeighbors()
    {
        MapDirection(HexTileDirection.RightTop.Id(), MyTilePosition + Vector3Int.up);
        MapDirection(HexTileDirection.RightMiddle.Id(), MyTilePosition + Vector3Int.right);
        MapDirection(HexTileDirection.RightBottom.Id(), MyTilePosition + Vector3Int.down);
        MapDirection(HexTileDirection.LeftBottom.Id(), MyTilePosition + Vector3Int.down + Vector3Int.left);
        MapDirection(HexTileDirection.LeftMiddle.Id(), MyTilePosition + Vector3Int.left);
        MapDirection(HexTileDirection.LeftTop.Id(), MyTilePosition + Vector3Int.up + Vector3Int.left);
    }

    private TileController MapDirection(int direction, Vector3Int tempKey)
    {
        TileController Sampler = neighborTiles[direction] = HexTileMap.ContainsKey(tempKey) ? HexTileMap[tempKey] : null;

        if (Displays.Length > direction && Displays[direction] != null)
        {
            Displays[direction].text = (Sampler != null ? "T" : "F");
        }

        return Sampler;
    }

    public void PlacePawn(PawnType pawn)
    {
        PawnData data = PawnTileDictionary.GetPawnData(pawn);

        PieceRenderer.sprite = data.SpriteImage;
        MyTilePawn = pawn;
    }

    public bool HasPawn()
    {
        return MyTilePawn != PawnType.NONE;
    }

    public bool HasBuilding()
    {
        return MyTileBuilding != StructureType.NONE;
    }

    //public bool CanPlacePiece(PawnType pawn)
    //{
    //    return (!HasBuilding() || pawn.CanBeat(MyTileBuilding)) && (!HasPawn() || pawn.CanBeat(MyTilePawn));
    //}
}
