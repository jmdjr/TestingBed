using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileController", menuName = "CustomTiles/TileController")]
public class TileControllerPiece : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        //if (go)
        //{
        //    var myTileObject = go.GetComponent<TileController>();

        //    if (myTileObject)
        //    {
        //        myTileObject.SetParams(position, tilemap, this);
        //    }
        //}
        return true;
    }
}
