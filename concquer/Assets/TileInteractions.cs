﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractions : MonoBehaviour
{
    public TileController Controller;

    private void OnMouseDown()
    {
        Debug.Log(Controller.GetTilePosition());
    }
}