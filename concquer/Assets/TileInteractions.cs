using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteractions : MonoBehaviour
{
    public TileController Controller;

    private void OnMouseDown()
    {
        if(!Controller.HasPawn())
        {
            Controller.PlacePawn(PawnType.PAWN);
        }
        else
        {
            Controller.PlacePawn(PawnType.NONE);
        }
    }
}
