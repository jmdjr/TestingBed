using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TwistTile", menuName = "CustomTiles/TwistTile")]
public class TwistTilePiece : Tile
{
    //public TwistTilePiece North;
    //public TwistTilePiece East;
    //public TwistTilePiece South;
    //public TwistTilePiece West;

    public TwistTile myTileObject;
    public Vector3Int myPosition;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);

    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //bool startup = base.StartUp(position, tilemap, go);

        //Vector3Int tempPos = new Vector3Int(-1, -1, 0);
        //Vector3Int tempPos = TilePosition;

        //North = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.up);
        //East = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.right);
        //South = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.down);
        //West = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.left);
        //if (startup)
        //{

        //North = (TwistTilePiece)tilemap.GetTile(tempPos);
        //East = (TwistTilePiece)tilemap.GetTile(tempPos);
        //South = (TwistTilePiece)tilemap.GetTile(tempPos);
        //West = (TwistTilePiece)tilemap.GetTile(tempPos);

        ////TwistTilePiece tNorth = Tilemap.GetTile<TwistTilePiece>(TilePosition + Vector3Int.up);
        ////TwistTilePiece tEast = Tilemap.GetTile<TwistTilePiece>(TilePosition + Vector3Int.right);
        ////TwistTilePiece tSouth = Tilemap.GetTile<TwistTilePiece>(TilePosition + Vector3Int.down);
        ////TwistTilePiece tWest = Tilemap.GetTile<TwistTilePiece>(TilePosition + Vector3Int.left);

        if (go)
        {
            myTileObject = go.GetComponent<TwistTile>();

            if (myTileObject)
            {
                myPosition = position;
                myTileObject.SetParams(position, tilemap, this);
                //Debug.Log($"{go.name}: {position}");
            }
        }
        //}

        //Debug.Log($"{position}: N - {North != null} E - {East != null} S - {South != null}  W - {West != null}");

        return true;
    }
}
