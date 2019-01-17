using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class PieceTypeData
{
    public static bool CanBeat(this PawnType typeA, PawnType typeB)
    {
        return ((int)typeA) > ((int)typeB);
    }

    public static bool CanBeat(this PawnType typeA, StructureType typeB)
    {
        return ((int)typeA) >= ((int)typeB);
    }

}
public enum PawnType
{
    NONE = 0,
    PAWN = NONE + 1,
    BISHOP = PAWN + 1,
    KNIGHT = BISHOP + 1,
    KING = KNIGHT + 1,
    SIZE = KING + 1
}

public enum StructureType
{
    NONE = 0,
    TREE = NONE + 1,
    HUT = TREE + 1,
    CASTLE = HUT + 1,
    SIZE = CASTLE + 1
}