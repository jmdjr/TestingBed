using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TwistTile", menuName = "CustomTiles/TwistTile")]
public class TwistTilePiece : Tile {

    private TwistTilePiece North;
    private TwistTilePiece East;
    private TwistTilePiece South;
    private TwistTilePiece West;

    TwistTile myTileObject;


    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        bool startup = base.StartUp(position, tilemap, go);

        North = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.up);
        East = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.right);
        South = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.down);
        West = (TwistTilePiece)tilemap.GetTile(position + Vector3Int.left);

        if(go)
        {
            myTileObject = go.GetComponent<TwistTile>();

            if (myTileObject)
            {
                myTileObject.setDirections(
                    North?.gameObject.GetComponent<TwistTile>(),
                    East?.gameObject.GetComponent<TwistTile>(),
                    South?.gameObject.GetComponent<TwistTile>(),
                    West?.gameObject.GetComponent<TwistTile>());
            }
        }

        //Debug.Log($"{position}: N - {North != null} E - {East != null} S - {South != null}  W - {West != null}");

        return startup;
    }

}
