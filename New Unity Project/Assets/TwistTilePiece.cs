using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TwistTile", menuName = "CustomTiles/TwistTile")]
public class TwistTilePiece : Tile {

    private TileBase North;
    private TileBase East;
    private TileBase South;
    private TileBase West;


    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        bool startup = base.StartUp(position, tilemap, go);

        North = tilemap.GetTile(position + Vector3Int.up);
        East = tilemap.GetTile(position + Vector3Int.right);
        South = tilemap.GetTile(position + Vector3Int.down);
        West = tilemap.GetTile(position + Vector3Int.left);

        //Debug.Log($"{position}: N - {North != null} E - {East != null} S - {South != null}  W - {West != null}");
        return startup;
    }

}
