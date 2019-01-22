using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexTileDirection
{
    RightTop = 0,
    RightMiddle,
    RightBottom,
    LeftBottom,
    LeftMiddle,
    LeftTop,
    Size
}

public static class HexTileDirectionHelper
{
    public static int Id(this HexTileDirection helper)
    {
        return (int)helper;
    }
}

