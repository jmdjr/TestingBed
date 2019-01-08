using System;
[Flags]
public enum TapDirections : uint
{
    North       = 1 << 0, 
    NorthEast   = 1 << 1,
    East        = 1 << 2,
    SouthEast   = 1 << 3,
    South       = 1 << 4,
    SouthWest   = 1 << 5,
    West        = 1 << 6,
    NorthWest   = 1 << 7
}

public static class TapDirectionsHelper
{
    public static TapDirections Opposite(this TapDirections helper)
    {
        return (TapDirections)(((uint)helper >> 4) | ((uint)helper << 4));
    }

    public static TapDirections RotateCounterClockwise(this TapDirections helper, int Steps)
    {
        return (TapDirections)(((uint)helper >> Steps) | ((uint)helper << (8 - Steps)));
    }

    public static TapDirections RotateClockwise(this TapDirections helper, int Steps)
    {
        return (TapDirections)(((uint)helper << Steps) | ((uint)helper >> (8 - Steps)));
    }
}

