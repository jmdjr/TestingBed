using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TwistTileMap : MonoBehaviour
{
    Tilemap map;

    // Start is called before the first frame update
    void Start()
    {
        map = transform.GetComponent<Tilemap>();
        Debug.Log(map.size);
        TileBase firstTile = map.GetTile(Vector3Int.zero);

        if(firstTile)
        {
            Debug.Log(firstTile.name);
            
            Debug.Log(firstTile);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
