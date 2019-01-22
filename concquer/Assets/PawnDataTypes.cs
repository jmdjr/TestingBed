using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnTypeDictionary", menuName = "Conquer/Dictionary/PawnType")]
public class PawnDataTypes : ScriptableObject
{
    public PawnData[] PawnTypes = new PawnData[(int)PawnType.SIZE];

    public PawnData GetPawnData(PawnType pawn)
    {
        return PawnTypes[(int)pawn];
    }

    public void Awake()
    {
        for(int n = 0; n < (int)PawnType.SIZE; ++n)
        {
            PawnData temp = PawnTypes[n] = new PawnData(n * 10, n * 2);
            temp.TypeName = ((PawnType)n).ToString();
        }
    }

    public PawnData GetPawnType(PawnType type)
    {
        return PawnTypes.FirstOrDefault(data => data.Type == type);
    }
}
