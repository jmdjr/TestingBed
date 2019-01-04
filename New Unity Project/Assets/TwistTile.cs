using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TwistTile : MonoBehaviour
{
    [Flags]
    public enum TapDirections
    {
        North = 0 >> 1,
        East =  1 >> 1,
        South = 2 >> 1,
        West =  3 >> 1
    }
    
    Transform Graphic;

    public TapDirections Directions;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
