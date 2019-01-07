using System;
[Flags]
public enum TapDirections
{
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3
}

public static class TapDirectionsHelper
{
    public static TapDirections RotateCounterClockwise(this TapDirections helper)
    {
        TapDirections rotatedDirections = 0;

        if (helper.HasFlag(TapDirections.East))  rotatedDirections |= TapDirections.North;
        if (helper.HasFlag(TapDirections.North)) rotatedDirections |= TapDirections.West;
        if (helper.HasFlag(TapDirections.West))  rotatedDirections |= TapDirections.South;
        if (helper.HasFlag(TapDirections.South)) rotatedDirections |= TapDirections.East;

        helper = rotatedDirections;

        return helper;
    }

    public static TapDirections RotateClockwise(this TapDirections helper)
    {
        TapDirections rotatedDirections = 0;

        if (helper.HasFlag(TapDirections.North)) rotatedDirections |= TapDirections.East;
        if (helper.HasFlag(TapDirections.West))  rotatedDirections |= TapDirections.North;
        if (helper.HasFlag(TapDirections.South)) rotatedDirections |= TapDirections.West;
        if (helper.HasFlag(TapDirections.East))  rotatedDirections |= TapDirections.South;

        helper = rotatedDirections;

        return helper;
    }
}

